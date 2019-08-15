using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace OODataBase_ClassLibrary
{
    public class DBManager
    {
        private DBPersister dbPersister;

        private static Dictionary<string, Dictionary<int, VersionsList>> TablesList;
        private Dictionary<string, List<string>> ParentChildren;

        private List<Tuple<bool, int, ReaderWriterLockSlim>> LockedList;

        private static int Version = 1;
        private static int TransactionID = 0;

        
        private static object syncInstance = new object();
        private static object syncCreate = new object();
        private static object syncLock = new object();
        private static object syncXML = new object();
        private static object syncGetTables = new object();
        

        private static DBManager DBM_instance = null;

        private DBManager()
        {
            dbPersister = DBPersister.DBP_Instance;

            TablesList = new Dictionary<string, Dictionary<int, VersionsList>>();
            ParentChildren = new Dictionary<string, List<string>>();
            LockedList = new List<Tuple<bool, int, ReaderWriterLockSlim>>();

            Version = dbPersister.CreateTables(TablesList, ParentChildren);
        }

        public static DBManager DBM_Instance
        {
            get
            {
                if(DBM_instance == null)
                {
                    lock(syncInstance)
                    {
                        if (DBM_instance == null)
                        {
                            DBM_instance = new DBManager();
                        }
                    }
                }

                return DBM_instance;
            }
        }
        
        public int VersionProperty
        {
            get { return Version; }
        }
        

        public Dictionary<string, List<string>> GetParentChildren()
        {
            return ParentChildren;
        }

        public Dictionary<string, Dictionary<int, List<object>>> GetTablesList()
        {
            // forbid more threads to change TablesList while this method is running
            lock (syncGetTables)
            {
                Dictionary<string, Dictionary<int, List<object>>> dict = new Dictionary<string, Dictionary<int, List<object>>>();

                foreach (var kvp1 in TablesList)
                {
                    dict.Add(kvp1.Key, new Dictionary<int, List<object>>());

                    foreach (var kvp2 in kvp1.Value)
                    {
                        dict[kvp1.Key].Add(kvp2.Key, new List<object>());

                        foreach (var item in kvp2.Value.versionsList)
                        {
                            dict[kvp1.Key][kvp2.Key].Add(item);
                        }
                    }
                }
            

            return dict;
            }
        }
        
        


        public int Create(string name, object item, int transactionID)
        {
            int ret = -1;

            lock (syncCreate)
            {
                lock (syncGetTables)
                {
                    if (TablesList[name].Count != 0)
                    {
                        ((Item)item).ID = TablesList[name].Last().Key + 1;
                    }
                    else
                    {
                        ((Item)item).ID = 0;
                    }

                    ret = ((Item)item).ID;

                    TablesList[name].Add(((Item)item).ID, new VersionsList());

                    TablesList[name][ret].locker.EnterWriteLock();
                    if (TablesList[name][ret].locker.IsWriteLockHeld)
                    {
                        lock (syncLock)
                            LockedList.Add(Tuple.Create(true, transactionID, TablesList[name][ret].locker));


                    }
                    TablesList[name][((Item)item).ID].Create(item);
                }
            }

            return ret;
        }
        
        public void CreateFromRollback(string name, object item)
        {
            lock (syncCreate)
            {
                TablesList[name][((Item)item).ID].Create(item);

                List<string> changeThisXML = new List<string>();
                changeThisXML.Add(name);

                dbPersister.RefreshXml(changeThisXML, TablesList, syncGetTables);
            }
        }

        public bool Delete(string name, int id, int version)
        {
            bool ret = true;

            if (!TablesList.ContainsKey(name))
            {
                ret = false;
            }
            else if (!TablesList[name].ContainsKey(id))
            {
                ret = false;
            }
            else
            {
                lock (syncGetTables)
                {
                    object obj = TablesList[name][id].versionsList.Find(i => ((Item)i).Version == version);
                    if (obj != null)
                    {
                        if (!TablesList[name][id].Delete(((Item)obj).Version))
                        {
                            ret = false;
                        }
                    }
                    else
                    {
                        ret = false;
                    }
                }
            }

            return ret;
        }
        
        public object Update(string name, int id, object obj)
        {
            object lastValidVersion = null;
            
            ((Item)obj).ID = id;

            lock (syncGetTables)
            {

                lastValidVersion = TablesList[name][id].versionsList.FirstOrDefault();

                TablesList[name][id].Create(obj);
            }

            return lastValidVersion;
        }
        
        

        public bool LockItem(string name, int id, int transactionID, bool isWrite)
        {
            bool ret = true;

            if(!TablesList.ContainsKey(name))
            {
                ret = false;
            }
            else
            {
                if(!TablesList[name].ContainsKey(id))
                {
                    ret = false;
                }
                else
                {
                    try
                    {
                        if (isWrite)
                        {
                            if (!TablesList[name][id].locker.IsWriteLockHeld)
                            {
                                if (TablesList[name][id].locker.TryEnterWriteLock(5))
                                {
                                    lock(syncLock)
                                        LockedList.Add(Tuple.Create(isWrite, transactionID, TablesList[name][id].locker));
                                }
                                else
                                {
                                    ret = false;
                                }
                            }
                        }
                        else
                        {
                            if (!TablesList[name][id].locker.IsReadLockHeld)
                            {
                                if (TablesList[name][id].locker.TryEnterReadLock(5))
                                {
                                    lock (syncLock)
                                        LockedList.Add(Tuple.Create(isWrite, transactionID, TablesList[name][id].locker));
                                }
                                else
                                {
                                    ret = false;
                                }
                            }
                        }
                    }
                    catch
                    {
                        ret = false;
                    }
                }
            }

            return ret;
        }
        
        public void UnlockItems(int transactionID, int transactionVersion, List<string> changeTheseXMLs)
        {
            // forbid more threads to enter list 'lockedItem'
            lock (syncLock)
            {
                foreach (var lockedItem in LockedList)
                {
                    if (lockedItem.Item2 == transactionID)
                    {
                        if (lockedItem.Item1)
                        {
                            lockedItem.Item3.ExitWriteLock();
                        }
                        else
                        {
                            lockedItem.Item3.ExitReadLock();
                        }
                    }
                }

                LockedList.RemoveAll(x => x.Item2 == transactionID);
            }

            if(changeTheseXMLs.Count != 0)
            {
                dbPersister.RefreshXml(changeTheseXMLs, TablesList, syncGetTables);
            }

            if(transactionVersion > Version)
            {
                Version = transactionVersion;
                dbPersister.WriteVersion(Version);
            }
        }

        public int GetTransactionID()
        {
            return Interlocked.Increment(ref TransactionID);
        }
    }


    public class VersionsList
    {
        public ReaderWriterLockSlim locker { get; private set; }
        public List<object> versionsList { get; private set; }

        public VersionsList()
        {
            locker = new ReaderWriterLockSlim();
            versionsList = new List<object>();
        }

        public bool Create(object item, bool fillTable = false)
        {
            bool ret = false;

            if(locker.IsWriteLockHeld || fillTable)
            {
                versionsList.Insert(0, item);
                ret = true;
            }

            return ret;
        }


        public object Read(int version)
        {
            object ret = null;

            if (locker.IsReadLockHeld)
            {
                ret = versionsList.Find(item => ((Item)item).Version <= version);
            }

            return ret;
        }

        
        public bool Delete(int version)
        {
            bool ret = false;

            if (locker.IsWriteLockHeld)
            {
                object foundItem = versionsList.Find(item => ((Item)item).Version == version);

                if (versionsList.Remove(foundItem))
                {
                    ret = true;
                }
            }

            return ret;
        }

        public void OrderByVersion()
        {
            versionsList = versionsList.OrderByDescending(i => ((Item)i).Version).ToList();
        }
    }

}
