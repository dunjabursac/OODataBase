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
    /// Interaction logic for SArea.xaml
    /// </summary>
    public partial class SArea : Window
    {
        private DBManager DB;
        private List<object> AllItemsInDB;

        private List<object> SelectedComputers;
        private List<object> SelectedAppliances;
        private List<object> SelectedPhones;

        private int inputPrice;
        private string inputBrand = "";

        private int inputRAM;
        private int inputROM;
        private string inputProcessor = "";

        private int inputNoiseLevel;
        private string inputEnergyClass = "";
        
        private int inputSpeakerVolume;
        private string inputMicrophoneSensitivity = "";

        private string ChoosenType = "";

        private Computer currentComputer;
        private Appliances currentAppliance;
        private Phone currentPhone;

        public SArea(DBManager db, string choosenType)
        {
            InitializeComponent();

            title.Content = choosenType;
            ChoosenType = choosenType;

            DB = db;
            AllItemsInDB = DB.GetAllItems();

            if (choosenType == "Computer")
            {
                ram_label.Visibility = Visibility.Visible;
                ram.Visibility = Visibility.Visible;
                rom_label.Visibility = Visibility.Visible;
                rom.Visibility = Visibility.Visible;
                processor_label.Visibility = Visibility.Visible;
                processor.Visibility = Visibility.Visible;
            }
            else if(choosenType == "Appliances")
            {
                nl_label.Visibility = Visibility.Visible;
                noiseLevel.Visibility = Visibility.Visible;
                ec_label.Visibility = Visibility.Visible;
                energyClass.Visibility = Visibility.Visible;
            }
            else
            {
                // Phone

                sv_label.Visibility = Visibility.Visible;
                speakerVolume.Visibility = Visibility.Visible;
                ms_label.Visibility = Visibility.Visible;
                microphoneSensitivity.Visibility = Visibility.Visible;
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

            inputBrand = brand.Text;

            if (ChoosenType == "Computer")
            {
                SelectedComputers = new List<object>();

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

                inputProcessor = processor.Text;

                if (inputBrand == "")
                {
                    if(inputProcessor == "")
                    {
                        // ni brend ni procesor

                        foreach (var item in AllItemsInDB)
                        {
                            try
                            {
                                currentComputer = (Computer)item;
                            }
                            catch
                            {
                                continue;
                            }

                            if (currentComputer.Price <= inputPrice && currentComputer.RAM <= inputRAM && currentComputer.ROM <= inputROM)
                            {
                                SelectedComputers.Add(currentComputer);
                            }
                        }
                    }
                    else
                    {
                        // samo procesor

                        foreach (var item in AllItemsInDB)
                        {
                            try
                            {
                                currentComputer = (Computer)item;
                            }
                            catch
                            {
                                continue;
                            }

                            if (currentComputer.Processor == inputProcessor && currentComputer.Price <= inputPrice && currentComputer.RAM <= inputRAM && currentComputer.ROM <= inputROM)
                            {
                                SelectedComputers.Add(currentComputer);
                            }
                        }
                    }
                }
                else
                {
                    if (inputProcessor == "")
                    {
                        // samo brend

                        foreach (var item in AllItemsInDB)
                        {
                            try
                            {
                                currentComputer = (Computer)item;
                            }
                            catch
                            {
                                continue;
                            }

                            if (currentComputer.Brand == inputBrand && currentComputer.Price <= inputPrice && currentComputer.RAM <= inputRAM && currentComputer.ROM <= inputROM)
                            {
                                SelectedComputers.Add(currentComputer);
                            }
                        }
                    }
                    else
                    {
                        // i brend i procesor

                        foreach (var item in AllItemsInDB)
                        {
                            try
                            {
                                currentComputer = (Computer)item;
                            }
                            catch
                            {
                                continue;
                            }

                            if (currentComputer.Brand == inputBrand && currentComputer.Processor == inputProcessor && currentComputer.Processor == inputProcessor && currentComputer.Price <= inputPrice && currentComputer.RAM <= inputRAM && currentComputer.ROM <= inputROM)
                            {
                                SelectedComputers.Add(currentComputer);
                            }
                        }
                    }
                }

                
                ShowSelectedIems ssi = new ShowSelectedIems(SelectedComputers, "Computer");
                ssi.Show();
            }
            else if (ChoosenType == "Appliances")
            {
                SelectedAppliances = new List<object>();

                try
                {
                    inputNoiseLevel = Convert.ToInt32(noiseLevel.Text);
                }
                catch
                {
                    inputNoiseLevel = Int32.MaxValue;
                }

                inputEnergyClass = energyClass.Text;

                if (inputBrand == "")
                {
                    if (inputEnergyClass == "")
                    {
                        // ni brend ni energyClass

                        foreach (var item in AllItemsInDB)
                        {
                            try
                            {
                                currentAppliance = (Appliances)item;
                            }
                            catch
                            {
                                continue;
                            }

                            if (currentAppliance.Price <= inputPrice && currentAppliance.NoiseLevel <= inputNoiseLevel)
                            {
                                SelectedAppliances.Add(currentAppliance);
                            }
                        }
                    }
                    else
                    {
                        // samo energyClass

                        foreach (var item in AllItemsInDB)
                        {
                            try
                            {
                                currentAppliance = (Appliances)item;
                            }
                            catch
                            {
                                continue;
                            }

                            if (currentAppliance.EnergyClass == inputEnergyClass && currentAppliance.Price <= inputPrice && currentAppliance.NoiseLevel <= inputNoiseLevel)
                            {
                                SelectedAppliances.Add(currentAppliance);
                            }
                        }
                    }
                }
                else
                {
                    if (inputProcessor == "")
                    {
                        // samo brend

                        foreach (var item in AllItemsInDB)
                        {
                            try
                            {
                                currentAppliance = (Appliances)item;
                            }
                            catch
                            {
                                continue;
                            }

                            if (currentAppliance.Brand == inputBrand && currentAppliance.Price <= inputPrice && currentAppliance.NoiseLevel <= inputNoiseLevel)
                            {
                                SelectedAppliances.Add(currentAppliance);
                            }
                        }
                    }
                    else
                    {
                        // i brend i energyClass

                        foreach (var item in AllItemsInDB)
                        {
                            try
                            {
                                currentAppliance = (Appliances)item;
                            }
                            catch
                            {
                                continue;
                            }

                            if (currentAppliance.Brand == inputBrand && currentAppliance.EnergyClass == inputEnergyClass && currentAppliance.Price <= inputPrice && currentAppliance.NoiseLevel <= inputNoiseLevel)
                            {
                                SelectedAppliances.Add(currentAppliance);
                            }
                        }
                    }
                }


                ShowSelectedIems ssi = new ShowSelectedIems(SelectedAppliances, "Appliances");
                ssi.Show();
            }
            else
            {
                // Phone

                SelectedPhones = new List<object>();

                try
                {
                    inputSpeakerVolume = Convert.ToInt32(speakerVolume.Text);
                }
                catch
                {
                    inputSpeakerVolume = Int32.MaxValue;
                }

                inputMicrophoneSensitivity = microphoneSensitivity.Text;

                if (inputBrand == "")
                {
                    if (inputMicrophoneSensitivity == "")
                    {
                        // ni brend ni microphoneSensitivity

                        foreach (var item in AllItemsInDB)
                        {
                            try
                            {
                                currentPhone = (Phone)item;
                            }
                            catch
                            {
                                continue;
                            }

                            if (currentPhone.Price <= inputPrice && currentPhone.SpeakerVolume <= inputSpeakerVolume)
                            {
                                SelectedPhones.Add(currentPhone);
                            }
                        }
                    }
                    else
                    {
                        // samo microphoneSensitivity

                        foreach (var item in AllItemsInDB)
                        {
                            try
                            {
                                currentPhone = (Phone)item;
                            }
                            catch
                            {
                                continue;
                            }

                            if (currentPhone.MicrophoneSensitivity == inputMicrophoneSensitivity && currentPhone.Price <= inputPrice && currentPhone.SpeakerVolume <= inputSpeakerVolume)
                            {
                                SelectedPhones.Add(currentPhone);
                            }
                        }
                    }
                }
                else
                {
                    if (inputProcessor == "")
                    {
                        // samo brend

                        foreach (var item in AllItemsInDB)
                        {
                            try
                            {
                                currentPhone = (Phone)item;
                            }
                            catch
                            {
                                continue;
                            }

                            if (currentPhone.Brand == inputBrand && currentPhone.Price <= inputPrice && currentPhone.SpeakerVolume <= inputSpeakerVolume)
                            {
                                SelectedPhones.Add(currentPhone);
                            }
                        }
                    }
                    else
                    {
                        // i brend i microphoneSensitivity

                        foreach (var item in AllItemsInDB)
                        {
                            try
                            {
                                currentPhone = (Phone)item;
                            }
                            catch
                            {
                                continue;
                            }

                            if (currentPhone.Brand == inputBrand && currentPhone.MicrophoneSensitivity == inputMicrophoneSensitivity && currentPhone.Price <= inputPrice && currentPhone.SpeakerVolume <= inputSpeakerVolume)
                            {
                                SelectedPhones.Add(currentPhone);
                            }
                        }
                    }
                }


                ShowSelectedIems ssi = new ShowSelectedIems(SelectedPhones, "Phone");
                ssi.Show();
            }
        }
    }
}
