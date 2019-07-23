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
    /// Interaction logic for SAppliances.xaml
    /// </summary>
    public partial class SAppliances : Window
    {
        private DBManager DB;
        private List<object> AllItemsInDB;

        private List<object> SelectedCoolingDevices;
        private List<CoolingDevice> AllCoolingDevices;
        private List<CoolingDevice> tmpCoolingDevices;
        private List<object> SelectedMachines;
        private List<Machine> AllMachines;
        private List<Machine> tmpMachines;
        private List<object> SelectedCookings;
        private List<Cooking> AllCookings;
        private List<Cooking> tmpCookings;

        private int inputPrice;
        private string inputBrand = "";
        private int inputNoiseLevel;
        private string inputEnergyClass;

        private int inputMinCoolingTemperature;
        private int inputVolume;
        private int inputMaxTemperature;

        private string ChoosenType = "";

        private CoolingDevice currentCoolingDevice;
        private Machine currentMachine;
        private Cooking currentCooking;


        public SAppliances(DBManager db, string choosenType)
        {
            InitializeComponent();

            title.Content = choosenType;
            ChoosenType = choosenType;

            DB = db;
            AllItemsInDB = DB.GetAllItems();

            if(ChoosenType == "CoolingDevice")
            {
                minCoolingTemperature_label.Visibility = Visibility.Visible;
                minCoolingTemperature.Visibility = Visibility.Visible;
            }
            else if(ChoosenType == "Machine")
            {
                volume_label.Visibility = Visibility.Visible;
                volume.Visibility = Visibility.Visible;
            }
            else
            {
                // Cooking

                maxTemperature_label.Visibility = Visibility.Visible;
                maxTemperature.Visibility = Visibility.Visible;
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
                inputNoiseLevel = Convert.ToInt32(noiseLevel.Text);
            }
            catch
            {
                inputNoiseLevel = Int32.MaxValue;
            }

            inputBrand = brand.Text;
            inputEnergyClass = energyClass.Text;


            if (ChoosenType == "CoolingDevice")
            {
                SelectedCoolingDevices = new List<object>();
                AllCoolingDevices = new List<CoolingDevice>();

                try
                {
                    inputMinCoolingTemperature = Convert.ToInt32(minCoolingTemperature.Text);
                }
                catch
                {
                    inputMinCoolingTemperature = Int32.MaxValue;
                }

                foreach(var item in AllItemsInDB)
                {
                    try
                    {
                        currentCoolingDevice = (CoolingDevice)item;

                        if(inputBrand == "")
                        {
                            AllCoolingDevices.Add(currentCoolingDevice);
                        }
                        else
                        {
                            if(currentCoolingDevice.Brand == inputBrand)
                            {
                                AllCoolingDevices.Add(currentCoolingDevice);
                            }
                        }
                    }
                    catch
                    {
                        continue;
                    }
                }

                tmpCoolingDevices = new List<CoolingDevice>(AllCoolingDevices);

                foreach(var coolingDevice in tmpCoolingDevices)
                {
                    if(inputEnergyClass == "")
                    {
                        break;
                    }
                    else
                    {
                        if(coolingDevice.EnergyClass != inputEnergyClass)
                        {
                            AllCoolingDevices.Remove(coolingDevice);
                        }
                    }
                }
                

                foreach (var coolingDevice in AllCoolingDevices)
                {
                    if(coolingDevice.Price <= inputPrice && coolingDevice.NoiseLevel <= inputNoiseLevel && coolingDevice.MinCoolingTemperature <= inputMinCoolingTemperature)
                    {
                        SelectedCoolingDevices.Add(coolingDevice);
                    }
                }

                ShowSelectedIems ssi = new ShowSelectedIems(SelectedCoolingDevices, "CoolingDevice");
                ssi.Show();
            }
            else if (ChoosenType == "Machine")
            {
                SelectedMachines = new List<object>();
                AllMachines = new List<Machine>();

                try
                {
                    inputVolume = Convert.ToInt32(volume.Text);
                }
                catch
                {
                    inputVolume = Int32.MaxValue;
                }

                foreach (var item in AllItemsInDB)
                {
                    try
                    {
                        currentMachine = (Machine)item;

                        if (inputBrand == "")
                        {
                            AllMachines.Add(currentMachine);
                        }
                        else
                        {
                            if (currentMachine.Brand == inputBrand)
                            {
                                AllMachines.Add(currentMachine);
                            }
                        }
                    }
                    catch
                    {
                        continue;
                    }
                }

                tmpMachines = new List<Machine>(AllMachines);

                foreach (var machine in tmpMachines)
                {
                    if (inputEnergyClass == "")
                    {
                        break;
                    }
                    else
                    {
                        if (machine.EnergyClass != inputEnergyClass)
                        {
                            AllMachines.Remove(machine);
                        }
                    }
                }

                foreach (var machine in AllMachines)
                {
                    if (machine.Price <= inputPrice && machine.NoiseLevel <= inputNoiseLevel && machine.Volume <= inputVolume)
                    {
                        SelectedMachines.Add(machine);
                    }
                }

                ShowSelectedIems ssi = new ShowSelectedIems(SelectedMachines, "Machine");
                ssi.Show();
            }
            else
            {
                // Cooking

                SelectedCookings = new List<object>();
                AllCookings = new List<Cooking>();

                try
                {
                    inputMaxTemperature = Convert.ToInt32(maxTemperature.Text);
                }
                catch
                {
                    inputMaxTemperature = Int32.MaxValue;
                }

                foreach (var item in AllItemsInDB)
                {
                    try
                    {
                        currentCooking = (Cooking)item;

                        if (inputBrand == "")
                        {
                            AllCookings.Add(currentCooking);
                        }
                        else
                        {
                            if (currentCooking.Brand == inputBrand)
                            {
                                AllCookings.Add(currentCooking);
                            }
                        }
                    }
                    catch
                    {
                        continue;
                    }
                }

                tmpCookings = new List<Cooking>(AllCookings);

                foreach (var cooking in tmpCookings)
                {
                    if (inputEnergyClass == "")
                    {
                        break;
                    }
                    else
                    {
                        if (cooking.EnergyClass != inputEnergyClass)
                        {
                            AllCookings.Remove(cooking);
                        }
                    }
                }

                foreach (var cooking in AllCookings)
                {
                    if (cooking.Price <= inputPrice && cooking.NoiseLevel <= inputNoiseLevel && cooking.MaxTemperature <= inputMaxTemperature)
                    {
                        SelectedCookings.Add(cooking);
                    }
                }

                ShowSelectedIems ssi = new ShowSelectedIems(SelectedCookings, "Cooking");
                ssi.Show();
            }
        }
    }
}
