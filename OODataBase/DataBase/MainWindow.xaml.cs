using DataBase.AddItems;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        public List<string> Items { get; set; }

        public static Dictionary<string, List<string>> ParentChildren;


        public MainWindow()
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
                catch(Exception e1)
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
                    catch(Exception e2)
                    {
                        Console.WriteLine("Error:" + e2.Message);
                        continue;
                    }


                    Console.WriteLine("Error:" + e1.Message);
                    continue;
                }
            }

            Items = new List<string>(Tables.Keys);
            DataContext = this;
            InitializeComponent();
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
            string selected = comboBox_Items.SelectedItem.ToString();

            Type t = Type.GetType("DataBase.AddItems.Add" + selected);
            var addLaptop = (Window)Activator.CreateInstance(t, Tables);
            addLaptop.Show();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            // prosledi SelectItem-u liste area, category, option

            SelectItems selectItems = new SelectItems(Tables);
            selectItems.Show();
        }
    }
}
