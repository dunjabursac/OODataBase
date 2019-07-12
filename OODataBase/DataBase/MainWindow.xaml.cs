using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml;
using System.Xml.Schema;

namespace DataBase
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static Dictionary<string, Dictionary<int, object>> Tables;

        public MainWindow()
        {
            InitializeComponent();

            Tables = new Dictionary<string, Dictionary<int, object>>();

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


            for (int i=0; i<myschema.Items.Count; i++)
            {
                try
                {
                    if(((XmlSchemaElement)myschema.Items[i]).Name.Contains("Element"))
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
                catch(Exception e)
                {
                    Console.WriteLine("Error:" + e.Message);
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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            AddFridge addLaptop = new AddFridge(Tables);
            addLaptop.Show();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            SelectItems selectItems = new SelectItems(Tables);
            selectItems.Show();
        }
    }
}
