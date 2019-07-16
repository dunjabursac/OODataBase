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
    /// Interaction logic for AddOven.xaml
    /// </summary>
    public partial class AddOven : Window
    {
        public static Dictionary<string, Dictionary<int, object>> Tables;

        public AddOven(Dictionary<string, Dictionary<int, object>> tables)
        {
            Tables = tables;
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Oven oven = new Oven()
            {
                Price = Convert.ToInt32(price.Text),
                Brand = brand.Text,
                MaxTemperature = Convert.ToInt32(maxTemperature.Text),
                EnergyClass = energyClass.Text,
                Volume = Convert.ToInt32(volume.Text),
                NoiseLevel = Convert.ToInt32(noiseLevel.Text),
            };

            Tables["Oven"].Add(Tables["Oven"].Count, oven);

            XmlSerializer xs = new XmlSerializer(typeof(Oven));

            TextWriter txtWriter = new StreamWriter("Oven.xml", true);

            xs.Serialize(txtWriter, oven);

            txtWriter.Close();

            this.Close();
        }
    }
}
