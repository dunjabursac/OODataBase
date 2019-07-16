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
    /// Interaction logic for AddMicrowave.xaml
    /// </summary>
    public partial class AddMicrowave : Window
    {
        public static Dictionary<string, Dictionary<int, object>> Tables;
        public AddMicrowave(Dictionary<string, Dictionary<int, object>> tables)
        {
            Tables = tables;
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Microwave microwave = new Microwave()
            {
                Price = Convert.ToInt32(price.Text),
                Brand = brand.Text,
                MaxTemperature = Convert.ToInt32(maxTemperature.Text),
                EnergyClass = energyClass.Text,
                Managing = managing.Text,
                NoiseLevel = Convert.ToInt32(noiseLevel.Text),
                Volume = Convert.ToInt32(volume.Text)
            };

            Tables["Microwave"].Add(Tables["Microwave"].Count, microwave);

            XmlSerializer xs = new XmlSerializer(typeof(Microwave));

            TextWriter txtWriter = new StreamWriter("Microwave.xml", true);

            xs.Serialize(txtWriter, microwave);

            txtWriter.Close();

            this.Close();
        }
    }
}
