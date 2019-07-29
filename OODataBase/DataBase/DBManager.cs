using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace DataBase
{
    public class DBManager
    {
        private static Dictionary<string, Dictionary<int, List<object>>> TablesList;
        private static Dictionary<string, List<string>> ParentChildren;
        private static int Version = 1;

        private static object locker = new object();

        

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

                        Type type = Type.GetType($"DataBase.{currwntClassName}");
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


                    Console.WriteLine(e1.InnerException);
                    Console.WriteLine(e1.StackTrace);
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
                    Type t2 = Type.GetType("DataBase." + item.Key);
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
            bool ret = true;

            lock (TablesList[name])
            {
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
                    Type t = Type.GetType("DataBase." + name);
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


            lock (TablesList[name][id])
            {
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
                    object obj = TablesList[name][id].Find(i => ((Item)i).Version <= version);
                    if (!TablesList[name][id].Remove(obj))
                    {
                        ret = false;
                    }
                }
            }

            lock (locker)
            {
                StreamWriter streamWriter = new StreamWriter(name + ".xml");
                streamWriter.Close();

                RefreshXml(name);
            }
            
            return ret;
        }

        public object Read(string name, int id, int version)
        {
            object ret = new object();

            if (!TablesList.ContainsKey(name))
            {
                ret = null;
            }
            else if (!TablesList[name].ContainsKey(id))
            {
                ret = null;
            }
            else
            {
                ret = TablesList[name][id].Find(i => ((Item)i).Version <= version);
            }

            return ret;
        }

        public bool Update(string name, int id, object obj)
        {
            bool ret = true;

            lock(TablesList[name][id])
            {
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
                    ((Item)obj).ID = id;
                    ((Item)obj).Version = Version;
                    TablesList[name][id].Insert(0, obj);    // INSERT is used for adding the newest version at begining of the list of items
                    Version++;
                    WriteVersion(Version);
                }
            }

            lock(locker)
            {
                StreamWriter streamWriter = new StreamWriter(name + ".xml");
                streamWriter.Close();

                RefreshXml(name);
            }

            return ret;
        }

        public bool UpdateMultiple(List<string> names, List<int> ids, List<object> objects)
        {
            bool ret = true;

            if (names.Count == 0)
                ret = false;

            for (int i = 0; i < names.Count; i++)
            {
                if (!TablesList.ContainsKey(names[i]))
                {
                    ret = false;
                    break;
                }
                else if (!TablesList[names[i]].ContainsKey(ids[i]))
                {
                    ret = false;
                    break;
                }
            }

            if (ret)
            {
                for (int i = 0; i < names.Count; i++)
                {
                    lock(TablesList[names[i]][ids[i]])
                    {
                        ((Item)objects[i]).ID = ids[i];
                        ((Item)objects[i]).Version = Version;
                        TablesList[names[i]][ids[i]].Insert(0, objects[i]);
                    }
                }
                Version++;
                WriteVersion(Version);
            }

            foreach (string name in names)
            {
                lock(locker)
                {
                    StreamWriter streamWriter = new StreamWriter(name + ".xml");
                    streamWriter.Close();

                    RefreshXml(name);
                }
            }

            return ret;
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

                using (var stream = File.Open(filename, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None))
                {
                    XmlWriter xml = null;
                    Type t = Type.GetType("DataBase." + name);
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
        


        private void FindLeafRecursive(List<object> itemsByType, string choosenType)
        {
            if(TablesList.ContainsKey(choosenType))
            {
                foreach(var kvp in TablesList[choosenType])
                {
                    itemsByType.AddRange(kvp.Value);
                }
            }
            else
            {
                foreach(string name in ParentChildren[choosenType])
                {
                    FindLeafRecursive(itemsByType, name);
                }
            }
        }

        public List<object> Select(string choosenType, string property, string value, int version)
        {
            List<object> itemsByType = new List<object>();
            List<object> helpList = new List<object>();

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

            foreach(var currentItem in helpList)
            {
                if(!(leafID_choosenVersion.ContainsKey(currentItem.GetType().ToString() + ((Item)currentItem).ID.ToString())) && ((Item)currentItem).Version <= version)
                {
                    leafID_choosenVersion.Add(currentItem.GetType().ToString() + ((Item)currentItem).ID.ToString(), currentItem);
                }
            }

            return new List<object>(leafID_choosenVersion.Values);
        }
    }
}
