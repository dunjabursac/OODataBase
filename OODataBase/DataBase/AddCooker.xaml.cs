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
    /// Interaction logic for AddCooker.xaml
    /// </summary>
    public partial class AddCooker : Window
    {
        public static Dictionary<string, Dictionary<int, object>> Tables;

        public AddCooker(Dictionary<string, Dictionary<int, object>> tables)
        {
            Tables = tables;
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Cooker cooker = new Cooker()
            {
                Price = Convert.ToInt32(price.Text),
                Brand = brand.Text,
                MaxTemperature = Convert.ToInt32(maxTemperature.Text),
                EnergyClass = energyClass.Text,
                PanelType = panelType.Text,
                NoiseLevel = Convert.ToInt32(noiseLevel.Text),
            };

            Tables["Cooker"].Add(Tables["Cooker"].Count, cooker);

            XmlSerializer xs = new XmlSerializer(typeof(Cooker));

            TextWriter txtWriter = new StreamWriter("Cooker.xml", true);

            xs.Serialize(txtWriter, cooker);

            txtWriter.Close();

            this.Close();
        }
    }
}
