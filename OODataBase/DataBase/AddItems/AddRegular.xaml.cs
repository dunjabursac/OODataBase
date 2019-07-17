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
    /// Interaction logic for AddRegular.xaml
    /// </summary>
    public partial class AddRegular : Window
    {
        DBManager DB;

        public AddRegular(DBManager db)
        {
            DB = db;
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Regular regular = new Regular()
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
            };

            DB.Create("Regular", regular);

            this.Close();
        }
    }
}
