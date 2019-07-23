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
    /// Interaction logic for UCooking.xaml
    /// </summary>
    public partial class UCooking : Window
    {
        DBManager DB;
        int ID;
        string Name1;
        int Version;
        public UCooking(DBManager db, object obj, string name, int id)
        {
            DB = db;
            ID = id;
            Name1 = name;
            Version = ((Item)obj).Version;
            InitializeComponent();

            price.Text = ((Cooking)obj).Price.ToString();
            brand.Text = ((Cooking)obj).Brand;
            energyClass.Text = ((Cooking)obj).EnergyClass;
            noiseLevel.Text = ((Cooking)obj).NoiseLevel.ToString();
            NoiseLevel.Visibility = Visibility.Visible;
            noiseLevel.Visibility = Visibility.Visible;
            maxTemperature.Text = ((Cooking)obj).MaxTemperature.ToString();

            if (name == "Oven")
            {
                volume.Visibility = Visibility.Visible;
                volume.Text = ((Oven)obj).Volume.ToString();
                Volume.Visibility = Visibility.Visible;
            }
            else if (name == "Cooker")
            {
                panelType.Visibility = Visibility.Visible;
                panelType.Text = ((Cooker)obj).PanelType;
                PanelType.Visibility = Visibility.Visible;
            }
            else
            {
                managing.Visibility = Visibility.Visible;
                managing.Text = ((Microwave)obj).Managing;
                Managing.Visibility = Visibility.Visible;
                volume.Visibility = Visibility.Visible;
                volume.Text = ((Microwave)obj).Volume.ToString();
                Volume.Visibility = Visibility.Visible;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            bool ret = true;

            if (Name1 == "Oven")
            {
                Oven oven = new Oven()
                {
                    Price = Convert.ToInt32(price.Text),
                    Brand = brand.Text,
                    MaxTemperature = Convert.ToInt32(maxTemperature.Text),
                    EnergyClass = energyClass.Text,
                    Volume = Convert.ToInt32(volume.Text),
                    NoiseLevel = Convert.ToInt32(noiseLevel.Text),
                };

                if (!DB.Update(Name1, ID, oven))
                    ret = false;
            }
            else if (Name1 == "Cooker")
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

                if (!DB.Update(Name1, ID, cooker))
                    ret = false;
            }
            else
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

                if (!DB.Update(Name1, ID, microwave))
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
