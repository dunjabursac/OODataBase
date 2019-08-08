﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OODataBase_ClassLibrary
{
    public class Transaction
    {
        public static DBManager db;
        
        private Dictionary<string, Dictionary<int, List<object>>> TablesList_T;
        private Dictionary<string, List<string>> ParentChildren_T;

        private List<Tuple<string, object>> Operations;
        private List<Tuple<string, object>> InverseOperations;

        
        List<Tuple<string, int>> old_new_IDs = new List<Tuple<string, int>>();
        
        // splitted items: nameID, List of its versions in current Transaction
        private Dictionary<string, List<object>> versionControl;

        // items added in current Transaction
        // not in the DataBase - CAN'T LOCK THEM !
        private List<string> nameID_newlyAddedItems;
        
        private int transactionID;
        private int Version;

        private bool transactionValid = true;
        private bool transactionBegun = false;


        public Transaction()
        {
            db = DBManager.DBM_Instance;
            transactionID = db.GetTransactionID();

            TablesList_T = new Dictionary<string, Dictionary<int, List<object>>>();
            ParentChildren_T = db.GetParentChildren();

            Operations = new List<Tuple<string, object>>();
            InverseOperations = new List<Tuple<string, object>>();

            versionControl = new Dictionary<string, List<object>>();
            nameID_newlyAddedItems = new List<string>();
        }

        
        public bool Create(object item)
        {
            Console.WriteLine("Transaction_" + this.transactionID + " is creating item ...");

            bool ret = true;

            if (transactionValid && transactionBegun)
            {
                string name = item.GetType().ToString().Split('.').Last();

                if (!TablesList_T.ContainsKey(name))
                {
                    ret = false;
                    transactionValid = false;
                }
                else
                {
                    if (TablesList_T[name].Count != 0)
                    {
                        ((Item)item).ID = ((Item)TablesList_T[name].Last().Value.FirstOrDefault()).ID + 1;
                    }
                    else
                    {
                        ((Item)item).ID = 0;
                    }


                    ((Item)item).Version = Version - 1;
                    TablesList_T[name].Add(((Item)item).ID, new List<object>());
                    TablesList_T[name][((Item)item).ID].Add(item);

                    nameID_newlyAddedItems.Add(name + ((Item)item).ID.ToString());
                    Operations.Add(Tuple.Create("create", item));
                }
            }
            else
            {
                ret = false;
                transactionValid = false;
            }

            return ret;
        }

        public object Read(string name, int id, int version)
        {
            Console.WriteLine("Transaction_" + transactionID + " is reading item ...");

            object ret = new object();

            if (transactionValid && transactionBegun)
            {
                if (!TablesList_T.ContainsKey(name))
                {
                    ret = null;
                    transactionValid = false;
                }
                else if (!TablesList_T[name].ContainsKey(id))
                {
                    ret = null;
                    transactionValid = false;
                }
                else
                {
                    ret = TablesList_T[name][id].Find(i => ((Item)i).Version <= version);
                }
            }
            else
            {
                ret = null;
                transactionValid = false;
            }
                

            return ret;
        }

        public bool Update(object item)
        {
            Console.WriteLine("Transaction_" + transactionID + " is updating item ...");

            bool ret = true;

            if(transactionValid && transactionBegun)
            {
                string name = item.GetType().ToString().Split('.').Last();
                int id = ((Item)item).ID;
                // string opName;
                bool lockOK = true;

                // is that item already in the DataBase
                if(!nameID_newlyAddedItems.Contains(name + id.ToString()))
                {
                    lockOK = db.LockItem(name, id, transactionID, true);
                }

                if (lockOK)
                {
                    if (!versionControl.ContainsKey(name + id.ToString()))
                    {
                        versionControl.Add(name + id.ToString(), new List<object>());
                    }

                    ((Item)item).Version = Version + versionControl[name + id.ToString()].Count;

                    versionControl[name + id.ToString()].Add(item);
                        
                    (TablesList_T[name][id]).Insert(0, item);
                    Operations.Add(Tuple.Create("update", item));
                }
                else
                {
                    ret = false;
                    transactionValid = false;
                }
            }
            else
            {
                ret = false;
                transactionValid = false;
            }

            return ret;
        }

        public bool Delete(string name, int id, int version)
        {
            Console.WriteLine("Transaction_" + transactionID + " is deleting item ...");

            bool ret = true;

            if (transactionValid && transactionBegun)
            {
                if (!TablesList_T.ContainsKey(name))
                {
                    ret = false;
                    transactionValid = false;
                }
                else if (!TablesList_T[name].ContainsKey(id))
                {
                    ret = false;
                    transactionValid = false;
                }
                else
                {
                    bool lockOK = true;

                    // is that item already in the DataBase
                    if (!nameID_newlyAddedItems.Contains(name + id.ToString()))
                    {
                        lockOK = db.LockItem(name, id, transactionID, true);
                    }

                    if (lockOK)
                    {
                        object obj = TablesList_T[name][id].Find(i => ((Item)i).Version == version);
                        if (!TablesList_T[name][id].Remove(obj))
                        {
                            ret = false;
                            transactionValid = false;
                        }
                        else
                        {
                            Operations.Add(Tuple.Create("delete", obj));
                        }
                    }
                    else
                    {
                        ret = false;
                        transactionValid = false;
                    }
                }
            }
            else
            {
                ret = false;
                transactionValid = false;
            }

            return ret;
        }

        public List<object> Select(string choosenType, string property, string value, int version)
        {
            Console.WriteLine("Transaction_" + transactionID + " is selecting items ...");

            List<object> selectedItems = new List<object>();

            if (transactionValid && transactionBegun)
            {
                List<object> itemsByType = new List<object>();
                List<object> helpList = new List<object>();

                if(choosenType == "")
                {
                    choosenType = "Item";
                }
                FindLeafRecursive(itemsByType, choosenType);

                if (property == "")
                {
                    helpList = itemsByType;
                }
                else
                {
                    foreach (var item in itemsByType)
                    {
                        foreach (PropertyDescriptor descriptor in TypeDescriptor.GetProperties(item))
                        {
                            // selecti items by TYPE and PROPERTY

                            if (descriptor.Name == property)
                            {
                                if (descriptor.GetValue(item).ToString() == value)
                                {
                                    helpList.Add(item);
                                    break;
                                }
                            }
                        }
                    }
                }

                Dictionary<string, object> leafID_choosenVersion = new Dictionary<string, object>();

                foreach (var currentItem in helpList)
                {
                    // select newest or wanted VERSION

                    if (!(leafID_choosenVersion.ContainsKey(currentItem.GetType().ToString() + ((Item)currentItem).ID.ToString())) && ((Item)currentItem).Version <= version)
                    {
                        leafID_choosenVersion.Add(currentItem.GetType().ToString() + ((Item)currentItem).ID.ToString(), currentItem);
                    }
                }

                selectedItems = new List<object>(leafID_choosenVersion.Values);
            }

            return selectedItems;
        }


        private void FindLeafRecursive(List<object> itemsByType, string choosenType)
        {
            if (TablesList_T.ContainsKey(choosenType))
            {
                foreach (var kvp in TablesList_T[choosenType])
                {
                    itemsByType.AddRange(kvp.Value);
                }
            }
            else
            {
                foreach (string name in ParentChildren_T[choosenType])
                {
                    FindLeafRecursive(itemsByType, name);
                }
            }
        }


        public bool Begin()
        {
            // take snapshot of database
            // (read from .xml files)

            Console.WriteLine("Transaction_" + transactionID + " is BEGINING ...");

            bool ret = true;

            if(!transactionBegun)
            {
                TablesList_T = new Dictionary<string, Dictionary<int, List<object>>> (db.GetTablesList());
                transactionBegun = true;
                Version = db.VersionProperty;
            }
            else
            {
                ret = false;
                transactionValid = false;
            }

            return ret;
        }

        public bool Commit()
        {
            // at the end of Transaction
            // (every operation went well, write in .xml files)

            bool ret = true;

            if (transactionValid)
            {
                Console.WriteLine("Transaction_" + transactionID + " is COMMITING changes ...");

                string name;
                int id;
                int version;
                

                foreach (var operation in Operations)
                {
                    // calling methods from DBManager

                    name = operation.Item2.GetType().ToString().Split('.').Last();
                    id = ((Item)operation.Item2).ID;
                    version = ((Item)operation.Item2).Version;

                    switch (operation.Item1)
                    {
                        case "create":
                            int createdItemID = db.Create(name, operation.Item2, transactionID);
                            if (createdItemID == -1)
                            {
                                Rollback();
                                ret = false;
                                return ret;
                            }
                            else
                            {
                                old_new_IDs.Add(Tuple.Create(name + id.ToString(), createdItemID));

                                InverseOperations.Insert(0, Tuple.Create("delete", operation.Item2));
                            }
                            break;

                        case "delete":
                            bool localDelete = false;
                            foreach (var old_new_item in old_new_IDs)
                            {
                                if(old_new_item.Item1 == name + id.ToString())
                                {
                                    id = old_new_item.Item2;
                                    localDelete = true;
                                    break;
                                }
                            }

                            if (!db.Delete(name, id, version, localDelete))
                            {
                                Rollback();
                                ret = false;
                                return ret;
                            }
                            else
                            {
                                InverseOperations.Insert(0, Tuple.Create("create", operation.Item2));
                            }
                            break;

                        case "update":
                            bool localUpdate = false;
                            foreach (var old_new_item in old_new_IDs)
                            {
                                if (old_new_item.Item1 == name + id.ToString())
                                {
                                    id = old_new_item.Item2;
                                    localUpdate = true;
                                    break;
                                }
                            }

                            object lastValidItem = db.Update(name, id, operation.Item2, localUpdate);
                            if (lastValidItem == null)
                            {
                                Rollback();
                                ret = false;
                                return ret;
                            }
                            else
                            {

                                InverseOperations.Insert(0, Tuple.Create("delete", operation.Item2));
                            }
                            break;

                        default:
                            break;
                    }
                }
                
                int maxVersionOfTransaction = 0;

                foreach (var kvp in versionControl)
                {
                    if (kvp.Value.Count > maxVersionOfTransaction)
                    {
                        maxVersionOfTransaction = kvp.Value.Count;
                    }
                }

                Version += maxVersionOfTransaction;

                db.UnlockItems(transactionID, Version);
            }
            else
            {
                Console.WriteLine("Transaction_" + transactionID + " is NOT COMMITING changes ...");
            }

            // all operations from Transaction were successfull
            Reset();

            return ret;
        }

        public bool Rollback()
        {
            // at any point in Transaction
            // (some operation went wrong, go back to BEGIN state)

            Console.WriteLine("Transaction_" + transactionID + " is ROLLING BACK changes ...");

            string name;
            int id;
            int version;

            foreach (var operation in InverseOperations)
            {
                name = operation.Item2.GetType().ToString().Split('.').Last();
                id = ((Item)operation.Item2).ID;
                version = ((Item)operation.Item2).Version;

                switch (operation.Item1)
                {
                    case "create":
                        foreach (var old_new_item in old_new_IDs)
                        {
                            if (old_new_item.Item1 == name + id.ToString())
                            {
                                id = old_new_item.Item2;
                                break;
                            }
                        }
                        db.CreateFromRollback(name, operation.Item2);
                        break;

                    case "delete":
                        bool localUpdate = false;
                        foreach (var old_new_item in old_new_IDs)
                        {
                            if (old_new_item.Item1 == name + id.ToString())
                            {
                                id = old_new_item.Item2;
                                localUpdate = true;
                                break;
                            }
                        }
                        db.Delete(name, id, version, localUpdate);
                        break;

                    case "update":
                        db.Update(name, id, operation.Item2, true);
                        break;

                    default:
                        break;
                }
            }

            // reset version, because COMMIT failed
            Version = db.VersionProperty;

            db.UnlockItems(transactionID, Version);
            Reset();

            return true;
        }



        private void Reset()
        {
            Operations = new List<Tuple<string, object>>();
            InverseOperations = new List<Tuple<string, object>>();
            versionControl = new Dictionary<string, List<object>>();
            nameID_newlyAddedItems = new List<string>();
            old_new_IDs = new List<Tuple<string, int>>();

            transactionValid = true;
            transactionBegun = false;
        }
    }
}
