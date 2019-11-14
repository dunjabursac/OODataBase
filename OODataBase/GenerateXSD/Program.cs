using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GenerateXSD
{
    class Program
    {
        static void Main(string[] args)
        {
            StreamWriter streamWriter = new StreamWriter("Model.xsd");
            streamWriter.Write("<?xml version=\"1.0\"?>" + "\n" + " <xs:schema xmlns:xs=\"http://www.w3.org/2001/XMLSchema\"> ");
            streamWriter.WriteLine("\n");

            streamWriter.Write("<xs:complexType name=\"Item\">\n"+
                                    "<xs:sequence>\n"+
                                    "<xs:element name = \"ID\" type = \"xs:int\"/>\n"+
                                    "<xs:element name = \"Version\" type = \"xs:int\"/>\n"+
                                    "</xs:sequence>\n"+
                                    "</xs:complexType>");

            streamWriter.WriteLine();

            List<string> classes = ClassInput(streamWriter);

            if(classes.Count == 0)
            {
                classes.Add("Item");
            }

            int i = 0;

            foreach (string cl in classes)
            {
                streamWriter.WriteLine("\n<xs:element name=\"Element" + i + "\" type=\"" + cl + "\"/> ");
                i++;
            }

            streamWriter.WriteLine("\n</xs:schema >");
            streamWriter.Close();

            Process p = new Process();
            p.StartInfo.CreateNoWindow = false;
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.FileName = "cmd.exe";
            p.StartInfo.RedirectStandardInput = true;
            p.Start();

            p.StandardInput.WriteLine("\"C:\\Program Files (x86)\\Microsoft SDKs\\Windows\\v10.0A\\bin\\NETFX 4.6.1 Tools\\xsd.exe\" /classes /language:CS /namespace:OODataBase_ClassLibrary Model.xsd");
            p.StandardInput.WriteLine("exit");


            while (true)
            {
                if (p.HasExited)
                {
                    //File.Copy("Model.cs", "..\\..\\..\\OODataBase_ClassLibrary\\Model.cs", true);
                    break;
                }
                else
                {
                    Thread.Sleep(10);
                }
            }
        }

        static List<string> ClassInput(StreamWriter streamWriter)
        {
            string input = "";
            List<string> classes = new List<string>();
            List<string> primitiveDataTypes = new List<string>() { "string", "int", "boolean", "dateTime", "long", "short", "double", "float" };
            

            while (true)
            {
                Console.WriteLine("Enter class name (or 0 to finish input): ");
                input = Console.ReadLine();

                if (input == "0")
                {
                    break;
                }

                if (input == "Item")
                {
                    Console.WriteLine("'Item' is reserved word!");
                    continue;
                }

                classes.Add(input);
                streamWriter.Write("\n<xs:complexType name=\"" + input + "\">");

                do
                {
                    Console.WriteLine("Do you want this class to inherit other class (y/n): ");
                    input = Console.ReadLine();

                    if (input == "y" || input == "Y")
                    {
                        Console.WriteLine("Enter parent class name: ");
                        string input2 = Console.ReadLine();

                        // 'classes' is a List of leaf classes
                        classes.Remove(input2);

                        streamWriter.Write("\n<xs:complexContent>" + "\n" + "<xs:extension base=\"" + input2 + "\">");
                    }
                    else if (input == "n" || input == "N")
                    {
                        streamWriter.Write("\n<xs:complexContent>" + "\n" + "<xs:extension base=\"" + "Item" + "\">");
                    }
                    else
                    {
                        Console.WriteLine("Not a valid answer!");
                    }

                } while (input != "y" && input != "Y" && input != "n" && input != "N");

                streamWriter.Write("\n" + "<xs:sequence>" + "\n");

                while (true)
                {
                    Console.WriteLine("Enter field name (or 0 to finish field input): ");
                    input = Console.ReadLine();

                    if (input == "0")
                    {
                        break;
                    }

                    streamWriter.Write("<xs:element name=\"" + input + "\" ");

                    Console.WriteLine("Enter field type: ");
                    input = Console.ReadLine();
                    

                    if(primitiveDataTypes.Contains(input))
                    {
                        streamWriter.Write("type=\"xs:" + input + "\"/> " + "\n");
                    }
                    else
                    {
                        streamWriter.Write("type=\"" + input + "\"/> " + "\n");
                    }
                }

                streamWriter.Write("</xs:sequence>" + "\n");
                streamWriter.Write("</xs:extension>\n</xs:complexContent>");

                streamWriter.Write("\n</xs:complexType>" + "\n");
            }

            return classes;
        }
    }
}
