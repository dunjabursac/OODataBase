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

namespace DataBase
{
    /// <summary>
    /// Interaction logic for AddDesktop.xaml
    /// </summary>
    public partial class AddDesktop : Window
    {
        public static Dictionary<string, Dictionary<int, object>> Tables;

        public AddDesktop(Dictionary<string, Dictionary<int, object>> tables)
        {
            Tables = tables;
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Desktop desktop = new Desktop()
            {
                Price = Convert.ToInt32(price.Text),
                Brand = brand.Text,
                RAM = Convert.ToInt32(ram.Text),
                ROM = Convert.ToInt32(rom.Text),
                Processor = processor.Text,
                Type = type.Text,
                PowerSupply = Convert.ToInt32(powerSupply.Text)
            };

            Tables["Desktop"].Add(Tables["Desktop"].Count, desktop);

            XmlSerializer xs = new XmlSerializer(typeof(Desktop));

            TextWriter txtWriter = new StreamWriter("Desktop.xml", true);

            xs.Serialize(txtWriter, desktop);

            txtWriter.Close();

            this.Close();
        }
    }
}
