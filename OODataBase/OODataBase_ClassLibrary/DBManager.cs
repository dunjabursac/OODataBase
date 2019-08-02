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
        private Dictionary<string, Dictionary<int, List<object>>> TablesList;
        private Dictionary<string, List<string>> ParentChildren;

        private List<Tuple<bool, int, List<object>>> LockedList;

        private static int Version = 1;
        private static int TransactionID = 0;
        
        private static object TansactionID_lock = new object();

        

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
                    DBM_instance = new DBManager();
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

            TablesList = new Dictionary<string, Dictionary<int, List<object>>>();

            ParentChildren = new Dictionary<string, List<string>>();
            ParentChildren.Add("Item", new List<string>());

            LockedList = new List<Tuple<bool, int, List<object>>>();

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
                        
                        TablesList.Add(className, new Dictionary<int, List<object>>());
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

            using (StreamWriter stream = new StreamWriter("Version.txt", false))
            {
                stream.Write(version);
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
                                TablesList[item.Key].Add(((Item)obj).ID, new List<object>());
                            }
                            
                            TablesList[item.Key][((Item)obj).ID].Insert(0, obj);
                        }
                    }
                }
            }

            SortTableList();
        }

        void SortTableList()
        {
            // sorting items by versions - HIGHER to LOWER

            foreach (var item in TablesList)
            {
                Dictionary<int, List<object>> tmp = new Dictionary<int, List<object>>(item.Value);
                foreach (var value in tmp)
                {
                    TablesList[item.Key][value.Key] = value.Value.OrderByDescending(i => ((Item)i).Version).ToList();
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
            Dictionary<string, Dictionary<int, List<object>>> dict = new Dictionary<string, Dictionary<int, List<object>>>();

            foreach(var kvp1 in TablesList)
            {
                dict.Add(kvp1.Key, new Dictionary<int, List<object>>());

                foreach(var kvp2 in kvp1.Value)
                {
                    dict[kvp1.Key].Add(kvp2.Key, new List<object>());

                    foreach(var item in kvp2.Value)
                    {
                        dict[kvp1.Key][kvp2.Key].Add(item);
                    }
                }
            }


            return dict;
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
                    foreach (var listItem in kvp2.Value)
                    {
                        allItems.Add(listItem);
                    }
                }
            }

            return allItems;
        }



        public bool Create(string name, object item)
        {
            bool ret = false;

            lock (TablesList[name])
            {
                ret = true;

                if (TablesList[name].Count != 0)
                {
                    ((Item)item).ID = ((Item)TablesList[name].Last().Value.FirstOrDefault()).ID + 1;
                }
                else
                {
                    ((Item)item).ID = 0;
                }

                TablesList[name].Add(((Item)item).ID, new List<object>());
                TablesList[name][((Item)item).ID].Add(item);

                string filename = name + ".xml";
                const string wrapperTagName = "wrapper";
                string wrapperStartTag = string.Format("<{0}>", wrapperTagName);
                string wrapperEndTag = string.Format("</{0}>", wrapperTagName);

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
                        if (!Encoding.UTF8.GetString(buffer).StartsWith(wrapperEndTag))
                        {
                            ret = false;
                        }
                        else
                        {
                            stream.SetLength(stream.Length - bufferLength);
                        }
                    }

                    serializer.Serialize(xml, item);
                    xml.WriteRaw(wrapperEndTag);
                    xml.Close();
                }
            }

            return ret;
        }

        public bool Delete(string name, int id, int version)
        {
            bool ret = true;


            //if (!TablesList.ContainsKey(name))
            //{
            //    ret = false;
            //}
            //else if (!TablesList[name].ContainsKey(id))
            //{
            //    ret = false;
            //}
            //else
            //{
            if (Monitor.IsEntered(TablesList[name][id]))
            {
                object obj = TablesList[name][id].Find(i => ((Item)i).Version == version);
                if (!TablesList[name][id].Remove(obj))
                {
                    ret = false;
                }
                else
                {
                    RefreshXml(name);
                }
            }
            else
            {
                ret = false;
            }
            //}

            return ret;
        }
        
        public object Update(string name, int id, object obj)
        {
            object lastValidVersion = null;

            if (Monitor.IsEntered(TablesList[name][id]))
            {
                lastValidVersion = TablesList[name][id].FirstOrDefault();
                //((Item)obj).Version = ((Item)lastValidVersion).Version + 1;

                TablesList[name][id].Insert(0, obj);   
                RefreshXml(name);
            }

            return lastValidVersion;
        }

        //public bool UpdateMultiple(List<string> names, List<int> ids, List<object> objects)
        //{
        //    bool ret = true;

        //    if (names.Count == 0)
        //        ret = false;

        //    for (int i = 0; i < names.Count; i++)
        //    {
        //        if (!TablesList.ContainsKey(names[i]))
        //        {
        //            ret = false;
        //            break;
        //        }
        //        else if (!TablesList[names[i]].ContainsKey(ids[i]))
        //        {
        //            ret = false;
        //            break;
        //        }
        //    }

        //    if (ret)
        //    {
        //        for (int i = 0; i < names.Count; i++)
        //        {
        //            lock(TablesList[names[i]][ids[i]])
        //            {
        //                ((Item)objects[i]).ID = ids[i];
        //                ((Item)objects[i]).Version = Version;
        //                TablesList[names[i]][ids[i]].Insert(0, objects[i]);
        //            }
        //        }
        //        Version++;
        //        WriteVersion(Version);
        //    }

        //    foreach (string name in names)
        //    {
        //        RefreshXml(name);
        //    }

        //    return ret;
        //}
        

        void RefreshXml(string name)
        {
            // rewriting .xml file with updated data

            if (TablesList[name].Count != 0)
            {
                string filename = name + ".xml";
                const string wrapperTagName = "wrapper";
                string wrapperStartTag = string.Format("<{0}>", wrapperTagName);
                string wrapperEndTag = string.Format("</{0}>", wrapperTagName);

                using (var stream = File.Open(filename, FileMode.Create, FileAccess.ReadWrite, FileShare.None))
                {
                    XmlWriter xml = null;
                    Type t = Type.GetType("OODataBase_ClassLibrary." + name);
                    var serializer = new XmlSerializer(t);
                    xml = XmlWriter.Create(stream, new XmlWriterSettings { OmitXmlDeclaration = true, ConformanceLevel = ConformanceLevel.Fragment, CloseOutput = false });
                    
                    xml.WriteRaw(wrapperStartTag);

                    foreach (var item in TablesList[name])
                    {
                        foreach (var value in item.Value)
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


        public bool LockItem(string name, int id, int transactionID)
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
                    bool partOfTuple = false;

                    try
                    {
                        if (Monitor.TryEnter(TablesList[name][id], 5))
                        {
                            LockedList.Add(Tuple.Create(partOfTuple, transactionID, TablesList[name][id]));
                        }
                        else
                        {
                            ret = false;
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
            foreach (var lockedItem in LockedList)
            {
                if(lockedItem.Item2 == transactionID)
                {
                    Monitor.Exit(lockedItem.Item3);
                }
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
}
