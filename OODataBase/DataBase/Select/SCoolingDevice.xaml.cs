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
    /// Interaction logic for SCoolingDevice.xaml
    /// </summary>
    public partial class SCoolingDevice : Window
    {
        private DBManager DB;
        private List<object> AllItemsInDB;

        private List<object> SelectedAirConditioners;
        private List<AirConditioner> AllAirConditioners;
        private List<AirConditioner> tmpAirConditioners;
        private List<object> SelectedFridges;
        private List<Fridge> AllFridges;
        private List<Fridge> tmpFridges;
        private List<object> SelectedFreezers;
        private List<Freezer> AllFreezers;
        private List<Freezer> tmpFreezers;

        private int inputPrice;
        private string inputBrand = "";
        private int inputNoiseLevel;
        private string inputEnergyClass;
        private int inputMinCoolingTemperature;

        private int inputCoolingCapacity;
        private string inputType = "";
        private int inputVolumeFridge;
        private int inputVolumeFreezer;

        private string ChoosenType = "";

        private AirConditioner currentAirConditioner;
        private Fridge currentFridge;
        private Freezer currentFreezer;


        public SCoolingDevice(DBManager db, string choosenType)
        {
            InitializeComponent();

            title.Content = choosenType;
            ChoosenType = choosenType;

            DB = db;
            AllItemsInDB = DB.GetAllItems();


            if(ChoosenType == "AirConditioner")
            {
                cc_label.Visibility = Visibility.Visible;
                coolingCapacity.Visibility = Visibility.Visible;
            }
            else if(ChoosenType == "Fridge")
            {
                type_label.Visibility = Visibility.Visible;
                type.Visibility = Visibility.Visible;
                volumeFridge_label.Visibility = Visibility.Visible;
                volumeFridge.Visibility = Visibility.Visible;
            }
            else
            {
                // Freezer

                volumeFreezer_label.Visibility = Visibility.Visible;
                volumeFreezer.Visibility = Visibility.Visible;
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

            try
            {
                inputMinCoolingTemperature = Convert.ToInt32(minCoolingTemperature.Text);
            }
            catch
            {
                inputMinCoolingTemperature = Int32.MaxValue;
            }

            inputBrand = brand.Text;
            inputEnergyClass = energyClass.Text;


            if (ChoosenType == "AirConditioner")
            {
                SelectedAirConditioners = new List<object>();
                AllAirConditioners = new List<AirConditioner>();

                try
                {
                    inputCoolingCapacity = Convert.ToInt32(coolingCapacity.Text);
                }
                catch
                {
                    inputCoolingCapacity = Int32.MaxValue;
                }


                foreach (var item in AllItemsInDB)
                {
                    try
                    {
                        currentAirConditioner = (AirConditioner)item;

                        if (inputBrand == "")
                        {
                            AllAirConditioners.Add(currentAirConditioner);
                        }
                        else
                        {
                            if (currentAirConditioner.Brand == inputBrand)
                            {
                                AllAirConditioners.Add(currentAirConditioner);
                            }
                        }
                    }
                    catch
                    {
                        continue;
                    }
                }

                tmpAirConditioners = new List<AirConditioner>(AllAirConditioners);

                foreach (var airConditioner in tmpAirConditioners)
                {
                    if (inputEnergyClass == "")
                    {
                        break;
                    }
                    else
                    {
                        if (airConditioner.EnergyClass != inputEnergyClass)
                        {
                            AllAirConditioners.Remove(airConditioner);
                        }
                    }
                }


                foreach (var airConditioner in AllAirConditioners)
                {
                    if (airConditioner.Price <= inputPrice && airConditioner.NoiseLevel <= inputNoiseLevel && airConditioner.MinCoolingTemperature <= inputMinCoolingTemperature && airConditioner.CoolingCapacity <= inputCoolingCapacity)
                    {
                        SelectedAirConditioners.Add(airConditioner);
                    }
                }

                ShowSelectedIems ssi = new ShowSelectedIems(SelectedAirConditioners, "AirConditioner");
                ssi.Show();
            }
            else if (ChoosenType == "Fridge")
            {
                SelectedFridges = new List<object>();
                AllFridges = new List<Fridge>();

                try
                {
                    inputVolumeFridge = Convert.ToInt32(volumeFridge.Text);
                }
                catch
                {
                    inputVolumeFridge = Int32.MaxValue;
                }

                inputType = type.Text;


                foreach (var item in AllItemsInDB)
                {
                    try
                    {
                        currentFridge = (Fridge)item;

                        if (inputBrand == "")
                        {
                            AllFridges.Add(currentFridge);
                        }
                        else
                        {
                            if (currentFridge.Brand == inputBrand)
                            {
                                AllFridges.Add(currentFridge);
                            }
                        }
                    }
                    catch
                    {
                        continue;
                    }
                }

                tmpFridges = new List<Fridge>(AllFridges);

                foreach (var fridge in tmpFridges)
                {
                    if (inputEnergyClass == "")
                    {
                        break;
                    }
                    else
                    {
                        if (fridge.EnergyClass != inputEnergyClass)
                        {
                            AllFridges.Remove(fridge);
                        }
                    }
                }

                tmpFridges = new List<Fridge>(AllFridges);

                foreach (var fridge in tmpFridges)
                {
                    if (inputType == "")
                    {
                        break;
                    }
                    else
                    {
                        if (fridge.Type != inputType)
                        {
                            AllFridges.Remove(fridge);
                        }
                    }
                }


                foreach (var fridge in AllFridges)
                {
                    if (fridge.Price <= inputPrice && fridge.NoiseLevel <= inputNoiseLevel && fridge.MinCoolingTemperature <= inputMinCoolingTemperature && fridge.Volume <= inputVolumeFridge)
                    {
                        SelectedFridges.Add(fridge);
                    }
                }

                ShowSelectedIems ssi = new ShowSelectedIems(SelectedFridges, "Fridge");
                ssi.Show();
            }
            else
            {
                // Freezer

                SelectedFreezers = new List<object>();
                AllFreezers = new List<Freezer>();

                try
                {
                    inputVolumeFreezer = Convert.ToInt32(volumeFreezer.Text);
                }
                catch
                {
                    inputVolumeFreezer = Int32.MaxValue;
                }


                foreach (var item in AllItemsInDB)
                {
                    try
                    {
                        currentFreezer = (Freezer)item;

                        if (inputBrand == "")
                        {
                            AllFreezers.Add(currentFreezer);
                        }
                        else
                        {
                            if (currentFreezer.Brand == inputBrand)
                            {
                                AllFreezers.Add(currentFreezer);
                            }
                        }
                    }
                    catch
                    {
                        continue;
                    }
                }

                tmpFreezers = new List<Freezer>(AllFreezers);

                foreach (var freezer in tmpFreezers)
                {
                    if (inputEnergyClass == "")
                    {
                        break;
                    }
                    else
                    {
                        if (freezer.EnergyClass != inputEnergyClass)
                        {
                            AllFreezers.Remove(freezer);
                        }
                    }
                }


                foreach (var freezer in AllFreezers)
                {
                    if (freezer.Price <= inputPrice && freezer.NoiseLevel <= inputNoiseLevel && freezer.MinCoolingTemperature <= inputMinCoolingTemperature && freezer.Volume <= inputVolumeFreezer)
                    {
                        SelectedFreezers.Add(freezer);
                    }
                }

                ShowSelectedIems ssi = new ShowSelectedIems(SelectedFreezers, "Freezer");
                ssi.Show();
            }
        }
    }
}
