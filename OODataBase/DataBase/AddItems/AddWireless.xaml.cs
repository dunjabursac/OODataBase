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
    /// Interaction logic for AddWireless.xaml
    /// </summary>
    public partial class AddWireless : Window
    {
        DBManager DB;

        public AddWireless(DBManager db)
        {
            DB = db;
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Wireless wireless = new Wireless()
            {
                Price = Convert.ToInt32(price.Text),
                Brand = brand.Text,
                MicrophoneSensitivity = microphoneSensitivity.Text,
                SpeakerVolume = Convert.ToInt32(speakerVolume.Text),
                BatteryCapacity = Convert.ToInt32(batteryCapacity.Text),
                Range = Convert.ToInt32(range.Text)
            };

            DB.Create("Wireless", wireless);

            this.Close();
        }
    }
}
