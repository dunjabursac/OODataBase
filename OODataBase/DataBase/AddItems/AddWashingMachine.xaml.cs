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
    /// Interaction logic for AddWashingMachine.xaml
    /// </summary>
    public partial class AddWashingMachine : Window
    {
        public static Dictionary<string, Dictionary<int, object>> Tables;

        public AddWashingMachine(Dictionary<string, Dictionary<int, object>> tables)
        {
            Tables = tables;
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            WashingMachine washingMachine = new WashingMachine()
            {
                Price = Convert.ToInt32(price.Text),
                Brand = brand.Text,
                TubDiameter = Convert.ToInt32(tubDiameter.Text),
                EnergyClass = energyClass.Text,
                Volume = Convert.ToInt32(volume.Text),
                NoiseLevel = Convert.ToInt32(noiseLevel.Text),
            };

            Tables["WashingMachine"].Add(Tables["WashingMachine"].Count, washingMachine);

            XmlSerializer xs = new XmlSerializer(typeof(WashingMachine));

            TextWriter txtWriter = new StreamWriter("WashingMachine.xml", true);

            xs.Serialize(txtWriter, washingMachine);

            txtWriter.Close();

            this.Close();
        }
    }
}
