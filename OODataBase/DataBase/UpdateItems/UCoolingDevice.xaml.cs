using System;
using System.Collections.Generic;
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

namespace DataBase.UpdateItems
{
    /// <summary>
    /// Interaction logic for UCoolingDevice.xaml
    /// </summary>
    public partial class UCoolingDevice : Window
    {
        DBManager DB;
        int ID;
        string Name1;
        int Version;
        public UCoolingDevice(DBManager db, object obj, string name, int id)
        {
            DB = db;
            ID = id;
            Name1 = name;
            Version = ((Item)obj).Version;
            InitializeComponent();

            price.Text = ((CoolingDevice)obj).Price.ToString();
            brand.Text = ((CoolingDevice)obj).Brand;
            minCoolingTemperature.Text = ((CoolingDevice)obj).MinCoolingTemperature.ToString();
            energyClass.Text = ((CoolingDevice)obj).EnergyClass;
            noiseLevel.Text = ((CoolingDevice)obj).NoiseLevel.ToString();

            if (name == "Fridge")
            {
                Volume.Visibility = Visibility.Visible;
                volume.Visibility = Visibility.Visible;
                volume.Text = ((Fridge)obj).Volume.ToString();
                Type.Visibility = Visibility.Visible;
                type.Visibility = Visibility.Visible;
                type.Text = ((Fridge)obj).Type;
            }
            else if (name == "Freezer")
            {
                Volume.Visibility = Visibility.Visible;
                volume.Visibility = Visibility.Visible;
                volume.Text = ((Freezer)obj).Volume.ToString();
            }
            else
            {
                CoolingCapacity.Visibility = Visibility.Visible;
                coolingCapacity.Visibility = Visibility.Visible;
                coolingCapacity.Text = ((AirConditioner)obj).CoolingCapacity.ToString();
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            bool ret = true;

            if (Name1 == "Fridge")
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

                if (!DB.Update(Name1, ID, fridge))
                    ret = false;
            }
            else if (Name1 == "Freezer")
            {
                Freezer freezer = new Freezer()
                {
                    Price = Convert.ToInt32(price.Text),
                    Brand = brand.Text,
                    MinCoolingTemperature = Convert.ToInt32(minCoolingTemperature.Text),
                    EnergyClass = energyClass.Text,
                    Volume = Convert.ToInt32(volume.Text),
                    NoiseLevel = Convert.ToInt32(noiseLevel.Text)
                };

                if (!DB.Update(Name1, ID, freezer))
                    ret = false;
            }
            else
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

                if (!DB.Update(Name1, ID, airConditioner))
                    ret = false;
            }

            if (!ret)
            {
                MessageBoxResult result = MessageBox.Show("Unable to update item!",
                                                  "Information",
                                                  MessageBoxButton.OK,
                                                  MessageBoxImage.Error);
                if (result == MessageBoxResult.OK)
                {
                    this.Close();
                }
            }
            else
            {
                MessageBoxResult result = MessageBox.Show("Updated successfully",
                                                  "Information",
                                                  MessageBoxButton.OK,
                                                  MessageBoxImage.Information);
                if (result == MessageBoxResult.OK)
                {
                    this.Close();
                }
            }
        }
    }
}
