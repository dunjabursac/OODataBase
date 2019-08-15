using System;
using System.Collections.Generic;
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
    public class DBPersister
    {
        private static DBPersister DBP_instance = null;
        private static object syncInstance = new object();

        private static int Version = 1;
        private static ReaderWriterLockSlim XML_lock = new ReaderWriterLockSlim();

        private DBPersister(){}

        public static DBPersister DBP_Instance
        {
            get
            {
                if (DBP_instance == null)
                {
                    lock (syncInstance)
                    {
                        if (DBP_instance == null)
                        {
                            DBP_instance = new DBPersister();
                        }
                    }
                }

                return DBP_instance;
            }
        }


        public int CreateTables(Dictionary<string, Dictionary<int, VersionsList>> TablesList, Dictionary<string, List<string>> ParentChildren)
        {
            if (File.Exists("Version.txt"))
            {
                using (StreamReader stream = new StreamReader("Version.txt"))
                {
                    if (!Int32.TryParse(stream.ReadLine(), out Version))
                    {
                        WriteVersion(Version);
                    }
                }
            }
            else
            {
                WriteVersion(Version);
            }
            
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
            
            CreateTableFrame(TablesList, ParentChildren, myschema);
            TableFill(TablesList);

            return Version;
        }

        void CreateTableFrame(Dictionary<string, Dictionary<int, VersionsList>> TablesList, Dictionary<string, List<string>> ParentChildren, XmlSchema myschema)
        {
            // creating "frame" to put data from DataBase in program memory

            foreach (var item in myschema.Items)
            {
                if ((item as XmlSchemaElement) != null)
                {
                    string className = ((XmlSchemaElement)item).SchemaTypeName.Name;
                    TablesList.Add(className, new Dictionary<int, VersionsList>());

                    if (!File.Exists(className + ".xml"))
                    {
                        StreamWriter streamWriter = new StreamWriter(className + ".xml");
                        streamWriter.Close();
                    }
                }
                else if ((item as XmlSchemaComplexType) != null)
                {
                    string currwntClassName = ((XmlSchemaComplexType)item).Name;
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
            }
        }

        public void WriteVersion(int version)
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

        static void ValidationCallback(object sender, ValidationEventArgs args)
        {
            // uses CreateTables() for reading data from .xsd file

            if (args.Severity == XmlSeverityType.Warning)
                Console.Write("WARNING: ");
            else if (args.Severity == XmlSeverityType.Error)
                Console.Write("ERROR: ");

            Console.WriteLine(args.Message);
        }

        void TableFill(Dictionary<string, Dictionary<int, VersionsList>> TablesList)
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

            SortTableList(TablesList);
        }

        void SortTableList(Dictionary<string, Dictionary<int, VersionsList>> TablesList)
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

        public void RefreshXml(List<string> changeTheseXMLs, Dictionary<string, Dictionary<int, VersionsList>> TablesList, object syncXML)
        {
            // rewriting .xml file with updated data

            foreach(var name in changeTheseXMLs)
            {
                string filename = name + ".xml";
                const string wrapperTagName = "wrapper";
                string wrapperStartTag = string.Format("<{0}>", wrapperTagName);
                string wrapperEndTag = string.Format("</{0}>", wrapperTagName);

                // locking entering certain .xml file
                lock (syncXML)
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
    }
}
