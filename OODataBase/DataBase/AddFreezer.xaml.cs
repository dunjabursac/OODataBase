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
    /// Interaction logic for AddFreezer.xaml
    /// </summary>
    public partial class AddFreezer : Window
    {
        public static Dictionary<string, Dictionary<int, object>> Tables;

        public AddFreezer(Dictionary<string, Dictionary<int, object>> tables)
        {
            Tables = tables;
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Freezer freezer = new Freezer()
            {
                Price = Convert.ToInt32(price.Text),
                Brand = brand.Text,
                MinCoolingTemperature = Convert.ToInt32(minCoolingTemperature.Text),
                EnergyClass = energyClass.Text,
                Volume = Convert.ToInt32(volume.Text),
                NoiseLevel = Convert.ToInt32(noiseLevel.Text)
            };

            Tables["Freezer"].Add(Tables["Freezer"].Count, freezer);

            XmlSerializer xs = new XmlSerializer(typeof(Freezer));

            TextWriter txtWriter = new StreamWriter("Freezer.xml", true);

            xs.Serialize(txtWriter, freezer);

            txtWriter.Close();

            this.Close();
        }
    }
}
