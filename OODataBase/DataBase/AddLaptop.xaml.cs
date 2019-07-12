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

namespace DataBase
{
    /// <summary>
    /// Interaction logic for AddLaptop.xaml
    /// </summary>
    public partial class AddLaptop : Window
    {
        public static Dictionary<string, Dictionary<int, object>> Tables;

        public AddLaptop(Dictionary<string, Dictionary<int, object>> tables)
        {
            Tables = tables;
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

            Tables["Laptop"].Add(Tables["Laptop"].Count, laptop);
        }
    }
}
