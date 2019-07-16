using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Xml.Serialization;

namespace DataBase.AddItems
{
    /// <summary>
    /// Interaction logic for AddTablet.xaml
    /// </summary>
    public partial class AddTablet : Window
    {
        public static Dictionary<string, Dictionary<int, object>> Tables;

        public AddTablet(Dictionary<string, Dictionary<int, object>> tables)
        {
            Tables = tables;
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Tablet tablet = new Tablet()
            {
                Price = Convert.ToInt32(price.Text),
                Brand = brand.Text,
                RAM = Convert.ToInt32(ram.Text),
                ROM = Convert.ToInt32(rom.Text),
                Processor = processor.Text,
                BatteryCapacity = Convert.ToInt32(batteryCapacity.Text),
                ScreenSize = Convert.ToInt32(screenSize.Text),
                Resolution = resolution.Text
            };

            Tables["Tablet"].Add(Tables["Tablet"].Count, tablet);

            XmlSerializer xs = new XmlSerializer(typeof(Tablet));

            TextWriter txtWriter = new StreamWriter("Tablet.xml", true);

            xs.Serialize(txtWriter, tablet);

            txtWriter.Close();

            this.Close();
        }
    }
}
