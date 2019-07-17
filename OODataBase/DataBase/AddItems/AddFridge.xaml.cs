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
    /// Interaction logic for AddFridge.xaml
    /// </summary>
    public partial class AddFridge : Window
    {
        DBManager DB;

        public AddFridge(DBManager db)
        {
            DB = db;
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Fridge fridge = new Fridge()
            {
                Price = Convert.ToInt32(price.Text),
                Brand = brand.Text,
                MinCoolingTemperature = Convert.ToInt32(minCoolingTemperature.Text),
                EnergyClass = energyClass.Text,
                Volume = Convert.ToInt32(volume.Text),
                NoiseLevel = Convert.ToInt32(noiseLevel.Text),
                Type = type.Text
            };

            DB.Create("Fridge", fridge);

            this.Close();
        }
    }
}
