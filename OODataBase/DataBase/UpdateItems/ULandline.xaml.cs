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
    /// Interaction logic for ULandline.xaml
    /// </summary>
    public partial class ULandline : Window
    {
        DBManager DB;
        int ID;
        string Name1;
        UMultipleItems PWindow = null;

        public ULandline(DBManager db, object obj, string name, int id, UMultipleItems window = null)
        {
            DB = db;
            ID = id;
            Name1 = name;
            PWindow = window;
            InitializeComponent();

            price.Text = ((Landline)obj).Price.ToString();
            brand.Text = ((Landline)obj).Brand;
            microphoneSensitivity.Text = ((Landline)obj).MicrophoneSensitivity;
            speakerVolume.Text = ((Landline)obj).SpeakerVolume.ToString();

            if (name == "Wire")
            {
                cableLength.Visibility = Visibility.Visible;
                cableLength.Text = ((Wire)obj).CableLength.ToString();
                CableLength.Visibility = Visibility.Visible;
            }
            else
            {
                batteryCapacity.Visibility = Visibility.Visible;
                batteryCapacity.Text = ((Wireless)obj).BatteryCapacity.ToString();
                BatteryCapacity.Visibility = Visibility.Visible;
                range.Visibility = Visibility.Visible;
                range.Text = ((Wireless)obj).Range.ToString();
                Range.Visibility = Visibility.Visible;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            bool ret = true;

            if (Name1 == "Wire")
            {
                Wire wire = new Wire()
                {
                    Price = Convert.ToInt32(price.Text),
                    Brand = brand.Text,
                    MicrophoneSensitivity = microphoneSensitivity.Text,
                    SpeakerVolume = Convert.ToInt32(speakerVolume.Text),
                    CableLength = Convert.ToInt32(cableLength.Text)
                };

                if (PWindow != null)
                {
                    PWindow.Updated(wire);
                }
                else
                {
                    if (!DB.Update(Name1, ID, wire))
                        ret = false;
                }
            }
            else
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

                if (PWindow != null)
                {
                    PWindow.Updated(wireless);
                }
                else
                {
                    if (!DB.Update(Name1, ID, wireless))
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
