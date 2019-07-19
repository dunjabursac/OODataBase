using System;
using System.Collections.Generic;
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
        public static Dictionary<string, Dictionary<int, object>> Tables;
        public static Dictionary<string, List<string>> ParentChildren;

        public DBManager()
        {
            CreateTables();
        }

        private void CreateTables()
        {
            Tables = new Dictionary<string, Dictionary<int, object>>();

            ParentChildren = new Dictionary<string, List<string>>();
            ParentChildren.Add("Item", new List<string>());

            XmlSchema myschema;

            try
            {
                XmlTextReader reader = new XmlTextReader("Model.xsd");
                myschema = XmlSchema.Read(reader, ValidationCallback);
                myschema.Write(Console.Out);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                myschema = null;
            }


            for (int i = 0; i < myschema.Items.Count; i++)
            {
                try
                {
                    if (((XmlSchemaElement)myschema.Items[i]).Name.Contains("Element"))
                    {
                        string className = ((XmlSchemaElement)myschema.Items[i]).SchemaTypeName.Name;

                        Tables.Add(className, new Dictionary<int, object>());
                        if (!File.Exists(className + ".xml"))
                        {
                            StreamWriter streamWriter = new StreamWriter(className + ".xml");
                            streamWriter.Close();
                        }
                    }
                }
                catch (Exception e1)
                {
                    // ucitavanje klasa iz XSD-a koje nisu listovi

                    try
                    {
                        string currwntClassName = ((XmlSchemaComplexType)myschema.Items[i]).Name;

                        Type type = Type.GetType($"DataBase.{currwntClassName}");
                        if (type != null && type.Name != "Item")
                        {
                            var currentClassInstance = Activator.CreateInstance(type);
                            var currentClassParentInstance = currentClassInstance.GetType().BaseType;//.GetGenericArguments()[0];
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
                        Console.WriteLine("Error:" + e2.Message);
                        continue;
                    }


                    Console.WriteLine("Error:" + e1.Message);
                    continue;
                }
            }

            foreach (var item in Tables)
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
                    int i = 0;

                    while (true)
                    {
                        obj = (object)xd.Deserialize(xml);
                        if (obj == null)
                            break;
                        xml.Read();
                        Tables[item.Key].Add(i, obj);
                        i++;
                    }
                }
            }
        }

        static void ValidationCallback(object sender, ValidationEventArgs args)
        {
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
            return new List<string>(Tables.Keys);
        }

        public bool Create(string name, object item)
        {
            Tables[name].Add(Tables[name].Count, item);

            bool ret = false;
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
                        throw new Exception("Invalid file");
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

            return ret;
        }

        public bool Delete(string name, int id)
        {
            bool ret = true;

            if (!Tables.ContainsKey(name))
            {
                ret = false;
            }
            else if (!Tables[name].ContainsKey(id))
            {
                ret = false;
            }
            else
            {
                if (!Tables[name].Remove(id))
                {
                    ret = false;
                }
            }

            StreamWriter streamWriter = new StreamWriter(name + ".xml");
            streamWriter.Close();

            foreach (var item in Tables[name])
            {
                Create(name, item.Value);
            }

            return ret;
        }

        public object Read(string name, int id)
        {
            object ret = new object();

            if (!Tables.ContainsKey(name))
            {
                ret = null;
            }
            else if (!Tables[name].ContainsKey(id))
            {
                ret = null;
            }
            else
            {
                ret = (Tables[name])[id];
            }

            return ret;
        }

        public bool Update(string name, int id, object obj)
        {
            bool ret = true;

            if (!Tables.ContainsKey(name))
            {
                ret = false;
            }
            else if (!Tables[name].ContainsKey(id))
            {
                ret = false;
            }
            else
            {
                Tables[name][id] = obj;
            }

            return ret;
        }
        
        public List<object> GetAllItems()
        {
            List<object> allItems = null;

            foreach(var kvp1 in Tables)
            {
                foreach (var kvp2 in kvp1.Value)
                {
                    allItems.Add(kvp2);
                }
            }

            return allItems;
        }
    }
}
