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
    /// Interaction logic for UMachine.xaml
    /// </summary>
    public partial class UMachine : Window
    {
        DBManager DB;
        int ID;
        string Name1;
        public UMachine(DBManager db, object obj, string name, int id)
        {
            DB = db;
            ID = id;
            Name1 = name;
            InitializeComponent();

            price.Text = ((Machine)obj).Price.ToString();
            brand.Text = ((Machine)obj).Brand;
            energyClass.Text = ((Machine)obj).EnergyClass;
            noiseLevel.Text = ((Machine)obj).NoiseLevel.ToString();
            volume.Text = ((Machine)obj).Volume.ToString();

            if (name == "Dishwasher")
            {
                numberOfLevels.Visibility = Visibility.Visible;
                numberOfLevels.Text = ((Dishwasher)obj).NumberOfLevels.ToString();
                NumberOfLevels.Visibility = Visibility.Visible;
            }
            else if (name == "WashingMachine")
            {
                tubDiameter.Visibility = Visibility.Visible;
                tubDiameter.Text = ((WashingMachine)obj).TubDiameter.ToString();
                TubDiameter.Visibility = Visibility.Visible;
            }
            else
            {
                dryingMode.Visibility = Visibility.Visible;
                dryingMode.Text = ((DryingMachine)obj).DryingMode;
                DryingMode.Visibility = Visibility.Visible;
                tubDiameter.Visibility = Visibility.Visible;
                tubDiameter.Text = ((DryingMachine)obj).TubDiameter.ToString();
                TubDiameter.Visibility = Visibility.Visible;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            bool ret = true;

            if (Name1 == "Dishwasher")
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

                if (!DB.Update(Name1, ID, dishwasher))
                    ret = false;
            }
            else if (Name1 == "WashingMachine")
            {
                WashingMachine washingMachine = new WashingMachine()
                {
                    Price = Convert.ToInt32(price.Text),
                    Brand = brand.Text,
                    TubDiameter = Convert.ToInt32(tubDiameter.Text),
                    EnergyClass = energyClass.Text,
                    Volume = Convert.ToInt32(volume.Text),
                    NoiseLevel = Convert.ToInt32(noiseLevel.Text),
                };

                if (!DB.Update(Name1, ID, washingMachine))
                    ret = false;
            }
            else
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

                if (!DB.Update(Name1, ID, dryingMachine))
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
