using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Schema;

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

        public void CreateTables()
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
        }

        static void ValidationCallback(object sender, ValidationEventArgs args)
        {
            if (args.Severity == XmlSeverityType.Warning)
                Console.Write("WARNING: ");
            else if (args.Severity == XmlSeverityType.Error)
                Console.Write("ERROR: ");

            Console.WriteLine(args.Message);
        }
    }
}
