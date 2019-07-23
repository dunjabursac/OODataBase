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
    /// Interaction logic for UMobile.xaml
    /// </summary>
    public partial class UMobile : Window
    {
        DBManager DB;
        int ID;
        string Name1;
        int Version;
        public UMobile(DBManager db, object obj, string name, int id)
        {
            DB = db;
            ID = id;
            Name1 = name;
            Version = ((Item)obj).Version;
            InitializeComponent();

            price.Text = ((Mobile)obj).Price.ToString();
            brand.Text = ((Mobile)obj).Brand;
            ram.Text = ((Mobile)obj).RAM.ToString();
            rom.Text = ((Mobile)obj).ROM.ToString();
            microphoneSensitivity.Text = ((Mobile)obj).MicrophoneSensitivity;
            os.Text = ((Mobile)obj).OS;
            resolution.Text = ((Mobile)obj).Resolution;
            screenSize.Text = ((Mobile)obj).ScreenSize.ToString();
            speakerVolume.Text = ((Mobile)obj).SpeakerVolume.ToString();

            if (name == "Smart")
            {
                WiFiType.Visibility = Visibility.Visible;
                wiFiType.Visibility = Visibility.Visible;
                wiFiType.Text = ((Smart)obj).WiFiType;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            bool ret = true;

            if (Name1 == "Smart")
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

                if (!DB.Update(Name1, ID, smart))
                    ret = false;
            }
            else
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

                if (!DB.Update(Name1, ID, regular))
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
