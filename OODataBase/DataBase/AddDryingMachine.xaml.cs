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
    /// Interaction logic for AddDryingMachine.xaml
    /// </summary>
    public partial class AddDryingMachine : Window
    {
        public static Dictionary<string, Dictionary<int, object>> Tables;

        public AddDryingMachine(Dictionary<string, Dictionary<int, object>> tables)
        {
            Tables = tables;
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DryingMachine dryingMachine = new DryingMachine()
            {
                Price = Convert.ToInt32(price.Text),
                Brand = brand.Text,
                TubDiameter = Convert.ToInt32(tubDiameter.Text),
                EnergyClass = energyClass.Text,
                Volume = Convert.ToInt32(volume.Text),
                NoiseLevel = Convert.ToInt32(noiseLevel.Text),
                DryingMode = dryingMode.Text
            };

            Tables["DryingMachine"].Add(Tables["DryingMachine"].Count, dryingMachine);

            XmlSerializer xs = new XmlSerializer(typeof(DryingMachine));

            TextWriter txtWriter = new StreamWriter("DryingMachine.xml", true);

            xs.Serialize(txtWriter, dryingMachine);

            txtWriter.Close();

            this.Close();
        }
    }
}
