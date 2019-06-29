using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenerateXSD
{
    class Program
    {
        static void Main(string[] args)
        {
            StreamWriter streamWriter = new StreamWriter("Model.xsd");
            streamWriter.Write("<?xml version=\"1.0\"?>" + "\n" + " <xs:schema xmlns:xs=\"http://www.w3.org/2001/XMLSchema\"> ");
            streamWriter.WriteLine();

            List<string> classes = ClassInput(streamWriter);

            int i = 0;

            foreach (string cl in classes)
            {
                streamWriter.WriteLine("\n<xs:element name=\"Element" + i + "\" type=\"" + cl + "\"/> ");
                i++;
            }

            streamWriter.WriteLine("\n</xs:schema >");
            streamWriter.Close();
        }

        static List<string> ClassInput(StreamWriter streamWriter)
        {
            string input = "";
            List<string> classes = new List<string>();
            bool inherit = false;

            while (true)
            {
                Console.WriteLine("Enter class name (or 0 to finish input): ");
                input = Console.ReadLine();

                if (input == "0")
                {
                    break;
                }

                classes.Add(input);

                streamWriter.Write("\n<xs:complexType name=\"" + input + "\">");

                Console.WriteLine("Do you want this class to inherit other class (y/n): ");
                input = Console.ReadLine();

                if (input == "y")
                {
                    Console.WriteLine("Enter parent class name: ");
                    input = Console.ReadLine();

                    classes.Remove(input);

                    streamWriter.Write("\n<xs:complexContent>" + "\n" + "<xs:extension base=\"" + input + "\">");
                    inherit = true;
                }

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

                    streamWriter.Write("type=\"xs:" + input + "\"/> " + "\n");
                }

                streamWriter.Write("</xs:sequence>" + "\n");

                if (inherit)
                {
                    streamWriter.Write("</xs:extension>\n</xs:complexContent>");
                    inherit = false;
                }

                streamWriter.Write("\n</xs:complexType>" + "\n");
            }

            return classes;
        }
    }
}
