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
    /// Interaction logic for AddDishwasher.xaml
    /// </summary>
    public partial class AddDishwasher : Window
    {
        DBManager DB;

        public AddDishwasher(DBManager db)
        {
            DB = db;
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

            DB.Create("Dishwasher", dishwasher);

            this.Close();
        }
    }
}
