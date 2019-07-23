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

namespace DataBase.Select
{
    /// <summary>
    /// Interaction logic for SPhone.xaml
    /// </summary>
    public partial class SPhone : Window
    {
        private DBManager DB;
        private List<object> AllItemsInDB;

        private List<object> SelectedLandlines;
        private List<Landline> AllLandlines;
        private List<Landline> tmpLandlines;
        private List<object> SelectedMobiles;
        private List<Mobile> AllMobiles;
        private List<Mobile> tmpMobiles;

        private int inputPrice;
        private string inputBrand = "";
        private int inputSpeakerVolume;
        private string inputMicrophoneSensitivity = "";

        private string inputOS;
        private int inputRAM;
        private int inputROM;
        private int inputScreenSize;
        private string inputResolution;

        private string ChoosenType = "";

        private Landline currentLandline;
        private Mobile currentMobile;

        public SPhone(DBManager db, string choosenType)
        {
            InitializeComponent();

            title.Content = choosenType;
            ChoosenType = choosenType;

            DB = db;
            AllItemsInDB = DB.GetAllItems();

            if(choosenType == "Mobile")
            {
                os_label.Visibility = Visibility.Visible;
                os.Visibility = Visibility.Visible;
                ram_label.Visibility = Visibility.Visible;
                ram.Visibility = Visibility.Visible;
                rom_label.Visibility = Visibility.Visible;
                rom.Visibility = Visibility.Visible;
                ss_label.Visibility = Visibility.Visible;
                screenSize.Visibility = Visibility.Visible;
                resolution_label.Visibility = Visibility.Visible;
                resolution.Visibility = Visibility.Visible;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();

            try
            {
                inputPrice = Convert.ToInt32(price.Text);
            }
            catch
            {
                inputPrice = Int32.MaxValue;
            }

            try
            {
                inputSpeakerVolume = Convert.ToInt32(speakerVolume.Text);
            }
            catch
            {
                inputSpeakerVolume = Int32.MaxValue;
            }

            inputBrand = brand.Text;
            inputMicrophoneSensitivity = microphoneSensitivity.Text;


            if(ChoosenType == "Mobile")
            {
                SelectedMobiles = new List<object>();
                AllMobiles = new List<Mobile>();

                try
                {
                    inputRAM = Convert.ToInt32(ram.Text);
                }
                catch
                {
                    inputRAM = Int32.MaxValue;
                }

                try
                {
                    inputROM = Convert.ToInt32(rom.Text);
                }
                catch
                {
                    inputROM = Int32.MaxValue;
                }

                try
                {
                    inputScreenSize = Convert.ToInt32(screenSize.Text);
                }
                catch
                {
                    inputScreenSize = Int32.MaxValue;
                }

                inputOS = os.Text;
                inputResolution = resolution.Text;


                foreach (var item in AllItemsInDB)
                {
                    try
                    {
                        currentMobile = (Mobile)item;

                        if (inputBrand == "")
                        {
                            AllMobiles.Add(currentMobile);
                        }
                        else
                        {
                            if (currentMobile.Brand == inputBrand)
                            {
                                AllMobiles.Add(currentMobile);
                            }
                        }
                    }
                    catch
                    {
                        continue;
                    }
                }

                tmpMobiles = new List<Mobile>(AllMobiles);

                foreach(var mobile in tmpMobiles)
                {
                    if (inputMicrophoneSensitivity == "")
                    {
                        break;
                    }
                    else
                    {
                        if (mobile.MicrophoneSensitivity != inputMicrophoneSensitivity)
                        {
                            AllMobiles.Remove(mobile);
                        }
                    }
                }

                tmpMobiles = new List<Mobile>(AllMobiles);

                foreach (var mobile in tmpMobiles)
                {
                    if (inputOS == "")
                    {
                        break;
                    }
                    else
                    {
                        if (mobile.OS != inputOS)
                        {
                            AllMobiles.Remove(mobile);
                        }
                    }
                }

                tmpMobiles = new List<Mobile>(AllMobiles);

                foreach (var mobile in tmpMobiles)
                {
                    if (inputResolution == "")
                    {
                        break;
                    }
                    else
                    {
                        if (mobile.Resolution != inputResolution)
                        {
                            AllMobiles.Remove(mobile);
                        }
                    }
                }


                foreach (var mobile in AllMobiles)
                {
                    if (mobile.Price <= inputPrice && mobile.SpeakerVolume <= inputSpeakerVolume && mobile.RAM <= inputRAM && mobile.ROM <= inputROM  && mobile.ScreenSize <= inputScreenSize)
                    {
                        SelectedMobiles.Add(mobile);
                    }
                }

                ShowSelectedIems ssi = new ShowSelectedIems(SelectedMobiles, "Mobile");
                ssi.Show();
            }
            else
            {
                // Landline

                SelectedLandlines = new List<object>();
                AllLandlines = new List<Landline>();

                foreach (var item in AllItemsInDB)
                {
                    try
                    {
                        currentLandline = (Landline)item;

                        if (inputBrand == "")
                        {
                            AllLandlines.Add(currentLandline);
                        }
                        else
                        {
                            if (currentLandline.Brand == inputBrand)
                            {
                                AllLandlines.Add(currentLandline);
                            }
                        }
                    }
                    catch
                    {
                        continue;
                    }
                }

                tmpLandlines = new List<Landline>(AllLandlines);

                foreach (var landline in tmpLandlines)
                {
                    if (inputMicrophoneSensitivity == "")
                    {
                        break;
                    }
                    else
                    {
                        if (landline.MicrophoneSensitivity != inputMicrophoneSensitivity)
                        {
                            AllLandlines.Remove(landline);
                        }
                    }
                }


                foreach (var landline in AllLandlines)
                {
                    if (landline.Price <= inputPrice && landline.SpeakerVolume <= inputSpeakerVolume)
                    {
                        SelectedLandlines.Add(landline);
                    }
                }

                ShowSelectedIems ssi = new ShowSelectedIems(SelectedLandlines, "Landline");
                ssi.Show();
            }
        }
    }
}
