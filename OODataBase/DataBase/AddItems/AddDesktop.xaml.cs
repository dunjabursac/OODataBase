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
    /// Interaction logic for AddDesktop.xaml
    /// </summary>
    public partial class AddDesktop : Window
    {
        DBManager DB;

        public AddDesktop(DBManager db)
        {
            DB = db;
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
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

            DB.Create("Desktop", desktop);

            this.Close();
        }
    }
}
