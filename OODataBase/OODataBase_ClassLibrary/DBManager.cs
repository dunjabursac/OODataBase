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
        private static Dictionary<string, Dictionary<int, VersionsList>> TablesList;
        private Dictionary<string, List<string>> ParentChildren;

        private List<Tuple<bool, int, ReaderWriterLockSlim>> LockedList;

        private static int Version = 1;
        private static int TransactionID = 0;

        private static ReaderWriterLockSlim XML_lock = new ReaderWriterLockSlim();
        private static object syncInstance = new object();
        private static object syncCreate = new object();
        private static object syncLock = new object();
        private static object syncXML = new object();
        private static object syncGetTables = new object();
        

        private static DBManager DBM_instance = null;

        private DBManager()
        {
            CreateTables();
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



        private void CreateTables()
        {
            try
            {
                using (StreamReader stream = new StreamReader("Version.txt"))
                {
                    Version = Int32.Parse(stream.ReadLine());
                }
            }
            catch
            {
                WriteVersion(Version);
            }

            TablesList = new Dictionary<string, Dictionary<int, VersionsList>>();

            ParentChildren = new Dictionary<string, List<string>>();
            ParentChildren.Add("Item", new List<string>());

            LockedList = new List<Tuple<bool, int, ReaderWriterLockSlim>>();

            // reading from .xsd file
            XmlSchema myschema;

            try
            {
                XmlTextReader reader = new XmlTextReader("Model.xsd");
                myschema = XmlSchema.Read(reader, ValidationCallback);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.InnerException);
                Console.WriteLine(e.StackTrace);
                myschema = null;
            }


            for (int i = 0; i < myschema.Items.Count; i++)
            {
                try
                {
                    if (((XmlSchemaElement)myschema.Items[i]).Name.Contains("Element"))
                    {
                        string className = ((XmlSchemaElement)myschema.Items[i]).SchemaTypeName.Name;
                        
                        TablesList.Add(className, new Dictionary<int, VersionsList>());
                        if (!File.Exists(className + ".xml"))
                        {
                            StreamWriter streamWriter = new StreamWriter(className + ".xml");
                            streamWriter.Close();
                        }
                    }
                }
                catch (Exception e1)
                {
                    // reading from .xsd file names of classes which are NOT leaves

                    try
                    {
                        string currwntClassName = ((XmlSchemaComplexType)myschema.Items[i]).Name;

                        Type type = Type.GetType($"OODataBase_ClassLibrary.{currwntClassName}");
                        if (type != null && type.Name != "Item")
                        {
                            var currentClassInstance = Activator.CreateInstance(type);
                            var currentClassParentInstance = currentClassInstance.GetType().BaseType;
                            string currentClassParentName = currentClassParentInstance.Name;

                            if (ParentChildren.ContainsKey(currentClassParentName))
                            {
                                ParentChildren[currentClassParentName].Add(currwntClassName);
                            }
                            else
                            {
                                ParentChildren.Add(currentClassParentName, new List<string>());
                                ParentChildren[currentClassParentName].Add(currwntClassName);
                            }
                        }
                    }
                    catch (Exception e2)
                    {
                        Console.WriteLine(e2.InnerException);
                        Console.WriteLine(e2.StackTrace);
                        continue;
                    }

                    //Console.WriteLine(e1.InnerException);
                    //Console.WriteLine(e1.StackTrace);
                    continue;
                }
            }

            TableFill();
        }
        
        void WriteVersion(int version)
        {
            // deleting current data in .txt file and write new data in it
            XML_lock.EnterWriteLock();

            try
            {
                using (StreamWriter stream = new StreamWriter("Version.txt", false))
                {
                    stream.Write(version);
                }
            }
            finally
            {
                XML_lock.ExitWriteLock();
            }
        }

        void TableFill()
        {
            // reading data from .xml file and putting it in TablesList

            foreach (var item in TablesList)
            {
                using (var xml = new XmlTextReader(item.Key + ".xml"))
                {
                    Type t2 = Type.GetType("OODataBase_ClassLibrary." + item.Key);
                    XmlSerializer xd = new XmlSerializer(t2);
                    try
                    {
                        xml.ReadStartElement("wrapper"); // skip the wrapper
                    }
                    catch
                    {
                        continue;
                    }
                    object obj;
                    int numOfTry = 0;

                    while (true)
                    {
                        obj = (object)xd.Deserialize(xml);
                        if (obj == null)
                        {
                            if (numOfTry == 1)
                            {
                                numOfTry = 0;
                                break;
                            }
                            
                            numOfTry++;
                            continue;

                        }
                        
                        if (obj != null)
                        {
                            numOfTry = 0;
                            if (!TablesList[item.Key].ContainsKey(((Item)obj).ID))
                            {
                                TablesList[item.Key].Add(((Item)obj).ID, new VersionsList());
                            }
                            
                            TablesList[item.Key][((Item)obj).ID].Create(obj, true);
                        }
                    }
                }
            }

            SortTableList();
        }

        void SortTableList()
        {
            // sorting items by versions - HIGHER to LOWER

            foreach (var liefType in TablesList)
            {
                foreach (var value in liefType.Value)
                {
                    TablesList[liefType.Key][value.Key].OrderByVersion();
                }
            }
        }

        static void ValidationCallback(object sender, ValidationEventArgs args)
        {
            // uses CreateTables() for reading data from .xsd file

            if (args.Severity == XmlSeverityType.Warning)
                Console.Write("WARNING: ");
            else if (args.Severity == XmlSeverityType.Error)
                Console.Write("ERROR: ");

            Console.WriteLine(args.Message);
        }

        

        public List<string> GetAllClassNames()
        {
            List<string> allClassNames = new List<string>();

            foreach(var cn in ParentChildren)
            {
                if(!allClassNames.Contains(cn.Key))
                {
                    allClassNames.Add(cn.Key);
                }

                foreach(var currentChild in cn.Value)
                {
                    if(!allClassNames.Contains(currentChild))
                    {
                        allClassNames.Add(currentChild);
                    }
                }
            }

            return allClassNames;
        }

        public Dictionary<string, List<string>> GetParentChildren()
        {
            return ParentChildren;
        }

        public Dictionary<string, Dictionary<int, List<object>>> GetTablesList()
        {
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

        public List<string> GetLeavesName()
        {
            return new List<string>(TablesList.Keys);
        }

        public List<object> GetAllItems()
        {
            List<object> allItems = new List<object>();

            foreach (var kvp1 in TablesList)
            {
                foreach (var kvp2 in kvp1.Value)
                {
                    foreach (var listItem in kvp2.Value.versionsList)
                    {
                        allItems.Add(listItem);
                    }
                }
            }

            return allItems;
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

                    RefreshXml(name);
                }
            }

            return ret;
        }

        
        public void CreateFromRollback(string name, object item)
        {
            lock (syncCreate)
            {
                TablesList[name][((Item)item).ID].Create(item);

                string filename = name + ".xml";
                const string wrapperTagName = "wrapper";
                string wrapperStartTag = string.Format("<{0}>", wrapperTagName);
                string wrapperEndTag = string.Format("</{0}>", wrapperTagName);


                lock (syncXML)
                {
                    using (var stream = File.Open(filename, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None))
                    {
                        XmlWriter xml = null;
                        Type t = Type.GetType("OODataBase_ClassLibrary." + name);
                        var serializer = new XmlSerializer(t);
                        if (stream.Length == 0)
                        {
                            xml = XmlWriter.Create(stream);
                            xml.WriteStartDocument();
                            xml.WriteRaw(wrapperStartTag);
                        }
                        else
                        {
                            xml = XmlWriter.Create(stream, new XmlWriterSettings { OmitXmlDeclaration = true });
                            var bufferLength = Encoding.UTF8.GetByteCount(wrapperEndTag);
                            var buffer = new byte[bufferLength];
                            stream.Position = stream.Length - bufferLength;
                            stream.Read(buffer, 0, bufferLength);
                            if (Encoding.UTF8.GetString(buffer).StartsWith(wrapperEndTag))
                            {
                                stream.SetLength(stream.Length - bufferLength);
                            }
                        }

                        serializer.Serialize(xml, item);
                        xml.WriteRaw(wrapperEndTag);
                        xml.Close();
                    }
                }
            }
        }

        public bool Delete(string name, int id, int version, bool localDelete)
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
                object obj = TablesList[name][id].versionsList.Find(i => ((Item)i).Version == version);
                if (!TablesList[name][id].Delete(((Item)obj).Version))
                {
                    ret = false;
                }
                else
                {
                    RefreshXml(name);
                }
            }

            return ret;
        }
        
        public object Update(string name, int id, object obj, bool localUpdate)
        {
            object lastValidVersion = null;
            
            ((Item)obj).ID = id;

            lastValidVersion = TablesList[name][id].versionsList.FirstOrDefault();

            TablesList[name][id].Create(obj);
            RefreshXml(name);

            return lastValidVersion;
        }
        
        

        void RefreshXml(string name)
        {
            // rewriting .xml file with updated data
            

            if (TablesList[name].Count != 0)
            {
                string filename = name + ".xml";
                const string wrapperTagName = "wrapper";
                string wrapperStartTag = string.Format("<{0}>", wrapperTagName);
                string wrapperEndTag = string.Format("</{0}>", wrapperTagName);

                lock(syncCreate)
                {
                    using (var stream = File.Open(filename, FileMode.Create, FileAccess.ReadWrite, FileShare.None))
                    {
                        XmlWriter xml = null;
                        Type t = Type.GetType("OODataBase_ClassLibrary." + name);
                        var serializer = new XmlSerializer(t);
                        xml = XmlWriter.Create(stream, new XmlWriterSettings { OmitXmlDeclaration = true, ConformanceLevel = ConformanceLevel.Fragment, CloseOutput = false });

                        xml.WriteRaw(wrapperStartTag);

                        foreach (var item in TablesList[name])
                        {
                            foreach (var value in item.Value.versionsList)
                            {
                                serializer.Serialize(xml, value);
                                xml.Flush();
                            }
                        }

                        xml.WriteRaw(wrapperEndTag);
                        xml.Close();
                    }
                }
            }
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

        

        public void UnlockItems(int transactionID, int transactionVersion)
        {
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

            if(transactionVersion > Version)
            {
                Version = transactionVersion;
                WriteVersion(Version);
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
