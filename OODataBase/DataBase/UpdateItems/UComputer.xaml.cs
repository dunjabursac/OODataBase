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
    /// Interaction logic for UComputer.xaml
    /// </summary>
    public partial class UComputer : Window
    {
        DBManager DB;
        int ID;
        string Name1;
        UMultipleItems PWindow = null;
        public UComputer(DBManager db, object obj, string name, int id, UMultipleItems window = null)
        {
            InitializeComponent();

            price.Text = ((Computer)obj).Price.ToString();
            brand.Text = ((Computer)obj).Brand;
            ram.Text = ((Computer)obj).RAM.ToString();
            rom.Text = ((Computer)obj).ROM.ToString();
            processor.Text = ((Computer)obj).Processor;

            if (name == "Laptop")
            {
                KeyboardType.Visibility = Visibility.Visible;
                BatteryCapacity.Visibility = Visibility.Visible;
                ScreenSize.Visibility = Visibility.Visible;
                Resolution.Visibility = Visibility.Visible;
                keyboardType.Visibility = Visibility.Visible;
                keyboardType.Text = ((Laptop)obj).KeyboardType;
                batteryCapacity.Visibility = Visibility.Visible;
                batteryCapacity.Text = ((Laptop)obj).BatteryCapacity.ToString();
                screenSize.Visibility = Visibility.Visible;
                screenSize.Text = ((Laptop)obj).ScreenSize.ToString();
                resolution.Visibility = Visibility.Visible;
                resolution.Text = ((Laptop)obj).Resolution;
            }
            else if (name == "Desktop")
            {
                Type.Visibility = Visibility.Visible;
                PowerSupply.Visibility = Visibility.Visible;
                type.Visibility = Visibility.Visible;
                type.Text = ((Desktop)obj).Type;
                powerSupply.Visibility = Visibility.Visible;
                powerSupply.Text = ((Desktop)obj).PowerSupply.ToString();
            }
            else
            {
                BatteryCapacity.Visibility = Visibility.Visible;
                ScreenSize.Visibility = Visibility.Visible;
                Resolution.Visibility = Visibility.Visible;
                batteryCapacity.Visibility = Visibility.Visible;
                batteryCapacity.Text = ((Tablet)obj).BatteryCapacity.ToString();
                screenSize.Visibility = Visibility.Visible;
                screenSize.Text = ((Tablet)obj).ScreenSize.ToString();
                resolution.Visibility = Visibility.Visible;
                resolution.Text = ((Tablet)obj).Resolution;
            }
            title.Content = name;
            DB = db;
            ID = id;
            PWindow = window;
            Name1 = name;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            bool ret = true;
            if (Name1 == "Laptop")
            {
                Laptop laptop = new Laptop()
                {
                    Price = Convert.ToInt32(price.Text),
                    Brand = brand.Text,
                    RAM = Convert.ToInt32(ram.Text),
                    ROM = Convert.ToInt32(rom.Text),
                    Processor = processor.Text,
                    KeyboardType = keyboardType.Text,
                    BatteryCapacity = Convert.ToInt32(batteryCapacity.Text),
                    ScreenSize = Convert.ToInt32(screenSize.Text),
                    Resolution = resolution.Text
                };

                if (PWindow != null)
                {
                    PWindow.Updated(laptop);
                }
                else
                {
                    if (!DB.Update(Name1, ID, laptop))
                        ret = false;
                }
            }
            else if (Name1 == "Desktop")
            {
                Desktop desktop = new Desktop()
                {
                    Price = Convert.ToInt32(price.Text),
                    Brand = brand.Text,
                    RAM = Convert.ToInt32(ram.Text),
                    ROM = Convert.ToInt32(rom.Text),
                    Processor = processor.Text,
                    Type = type.Text,
                    PowerSupply = Convert.ToInt32(powerSupply.Text)
                };

                if (PWindow != null)
                {
                    PWindow.Updated(desktop);
                }
                else
                {
                    if (!DB.Update(Name1, ID, desktop))
                        ret = false;
                }
            }
            else
            {
                Tablet tablet = new Tablet()
                {
                    Price = Convert.ToInt32(price.Text),
                    Brand = brand.Text,
                    RAM = Convert.ToInt32(ram.Text),
                    ROM = Convert.ToInt32(rom.Text),
                    Processor = processor.Text,
                    BatteryCapacity = Convert.ToInt32(batteryCapacity.Text),
                    ScreenSize = Convert.ToInt32(screenSize.Text),
                    Resolution = resolution.Text
                };

                if (PWindow != null)
                {
                    PWindow.Updated(tablet);
                }
                else
                {
                    if (!DB.Update(Name1, ID, tablet))
                        ret = false;
                }
            }

            if (PWindow == null)
            {
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
            else
            {
                this.Close();
            }
        }
    }
}
