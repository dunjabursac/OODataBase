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
        DBManager DB;

        public AddMicrowave(DBManager db)
        {
            DB = db;
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

            DB.Create("Microwave", microwave);

            this.Close();
        }
    }
}
