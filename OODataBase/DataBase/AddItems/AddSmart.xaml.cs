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
    /// Interaction logic for AddSmart.xaml
    /// </summary>
    public partial class AddSmart : Window
    {
        public static Dictionary<string, Dictionary<int, object>> Tables;

        public AddSmart(Dictionary<string, Dictionary<int, object>> tables)
        {
            Tables = tables;
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Smart smart = new Smart()
            {
                Price = Convert.ToInt32(price.Text),
                Brand = brand.Text,
                RAM = Convert.ToInt32(ram.Text),
                MicrophoneSensitivity = microphoneSensitivity.Text,
                ROM = Convert.ToInt32(rom.Text),
                OS = os.Text,
                Resolution = resolution.Text,
                ScreenSize = Convert.ToInt32(screenSize.Text),
                SpeakerVolume = Convert.ToInt32(speakerVolume.Text),
                WiFiType = wiFiType.Text,
            };

            Tables["Smart"].Add(Tables["Smart"].Count, smart);

            XmlSerializer xs = new XmlSerializer(typeof(Smart));

            TextWriter txtWriter = new StreamWriter("Smart.xml", true);

            xs.Serialize(txtWriter, smart);

            txtWriter.Close();

            this.Close();
        }
    }
}
