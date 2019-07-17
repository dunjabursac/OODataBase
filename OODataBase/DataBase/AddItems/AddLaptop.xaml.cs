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
    /// Interaction logic for AddLaptop.xaml
    /// </summary>
    public partial class AddLaptop : Window
    {
        DBManager DB;

        public AddLaptop(DBManager db)
        {
            DB = db;
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
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

            DB.Create("Laptop", laptop);

            this.Close();
        }
    }
}
