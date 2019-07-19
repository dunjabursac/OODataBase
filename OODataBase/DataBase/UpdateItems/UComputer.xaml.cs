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
        public UComputer(DBManager db, object obj, string name, int id)
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
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
