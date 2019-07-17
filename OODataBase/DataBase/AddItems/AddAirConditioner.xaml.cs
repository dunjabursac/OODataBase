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
    /// Interaction logic for AddAirConditioner.xaml
    /// </summary>
    public partial class AddAirConditioner : Window
    {
        DBManager DB;
        public AddAirConditioner(DBManager db)
        {
            DB = db;
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            AirConditioner airConditioner = new AirConditioner()
            {
                Price = Convert.ToInt32(price.Text),
                Brand = brand.Text,
                CoolingCapacity = Convert.ToInt32(coolingCapacity.Text),
                EnergyClass = energyClass.Text,
                MinCoolingTemperature = Convert.ToInt32(minCoolingTemperature.Text),
                NoiseLevel = Convert.ToInt32(noiseLevel.Text),
            };

            DB.Create("AirConditioner", airConditioner);

            this.Close();
        }
    }
}
