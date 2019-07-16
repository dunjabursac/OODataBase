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
        public static Dictionary<string, Dictionary<int, object>> Tables;

        public AddWireless(Dictionary<string, Dictionary<int, object>> tables)
        {
            Tables = tables;
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

            Tables["Wireless"].Add(Tables["Wireless"].Count, wireless);

            XmlSerializer xs = new XmlSerializer(typeof(Wireless));

            TextWriter txtWriter = new StreamWriter("Wireless.xml", true);

            xs.Serialize(txtWriter, wireless);

            txtWriter.Close();

            this.Close();
        }
    }
}
