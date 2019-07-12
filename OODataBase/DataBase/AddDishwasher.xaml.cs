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
    /// Interaction logic for AddDishwasher.xaml
    /// </summary>
    public partial class AddDishwasher : Window
    {
        public static Dictionary<string, Dictionary<int, object>> Tables;

        public AddDishwasher(Dictionary<string, Dictionary<int, object>> tables)
        {
            Tables = tables;
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Dishwasher dishwasher = new Dishwasher()
            {
                Price = Convert.ToInt32(price.Text),
                Brand = brand.Text,
                NumberOfLevels = Convert.ToInt32(numberOfLevels.Text),
                EnergyClass = energyClass.Text,
                Volume = Convert.ToInt32(volume.Text),
                NoiseLevel = Convert.ToInt32(noiseLevel.Text),
            };

            Tables["Dishwasher"].Add(Tables["Dishwasher"].Count, dishwasher);

            XmlSerializer xs = new XmlSerializer(typeof(Dishwasher));

            TextWriter txtWriter = new StreamWriter("Dishwasher.xml", true);

            xs.Serialize(txtWriter, dishwasher);

            txtWriter.Close();

            this.Close();
        }
    }
}
