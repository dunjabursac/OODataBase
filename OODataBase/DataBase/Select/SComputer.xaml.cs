﻿using System;
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
    /// Interaction logic for SComputer.xaml
    /// </summary>
    public partial class SComputer : Window
    {
        private DBManager DB;
        private List<object> AllItemsInDB;

        private List<object> SelectedLaptops;
        private List<Laptop> AllLaptops;
        private List<Laptop> tmp;
        private List<object> SelectedDesktops;
        private List<Desktop> AllDesktops;
        private List<object> SelectedTablets;
        private List<Tablet> AllTablets;

        private int inputPrice;
        private string inputBrand = "";
        private int inputRAM;
        private int inputROM;
        private string inputProcessor = "";

        private int inputBatteryCapacity;
        private int inputScreenSize;
        private string inputResolution;
        private string inputKeyboardType;

        private string inputType;
        private int inputPowerSupply;

        private int inputBatteryCapacity_T;
        private int inputScreenSize_T;
        private string inputResolution_T;

        private string ChoosenType = "";

        private Laptop currentLaptop;
        private Desktop currentDesktop;
        private Tablet currentTablet;

        public SComputer(DBManager db, string choosenType)
        {
            InitializeComponent();

            title.Content = choosenType;
            ChoosenType = choosenType;

            DB = db;
            AllItemsInDB = DB.GetAllItems();

            if(choosenType == "Laptop")
            {
                bc_label.Visibility = Visibility.Visible;
                batteryCapacity.Visibility = Visibility.Visible;
                ss_label.Visibility = Visibility.Visible;
                screenSize.Visibility = Visibility.Visible;
                resolution_label.Visibility = Visibility.Visible;
                resolution.Visibility = Visibility.Visible;
                kt_label.Visibility = Visibility.Visible;
                keyboardType.Visibility = Visibility.Visible;
            }
            else if(choosenType == "Desktop")
            {
                type_label.Visibility = Visibility.Visible;
                type.Visibility = Visibility.Visible;
                ps_label.Visibility = Visibility.Visible;
                powerSupply.Visibility = Visibility.Visible;
            }
            else
            {
                // Tablet

                bc_T_label.Visibility = Visibility.Visible;
                batteryCapacity_T.Visibility = Visibility.Visible;
                ss_T_label.Visibility = Visibility.Visible;
                screenSize_T.Visibility = Visibility.Visible;
                resolution_T_label.Visibility = Visibility.Visible;
                resolution_T.Visibility = Visibility.Visible;
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


            inputBrand = brand.Text;
            inputProcessor = processor.Text;


            if(ChoosenType == "Laptop")
            {
                SelectedLaptops = new List<object>();
                AllLaptops = new List<Laptop>();

                try
                {
                    inputBatteryCapacity = Convert.ToInt32(batteryCapacity.Text);
                    inputScreenSize = Convert.ToInt32(screenSize.Text);
                }
                catch
                {
                    inputBatteryCapacity = Int32.MaxValue;
                    inputScreenSize = Int32.MaxValue;
                }

                inputResolution = resolution.Text;
                inputKeyboardType = keyboardType.Text;

                foreach(var item in AllItemsInDB)
                {
                    try
                    {
                        currentLaptop = (Laptop)item;

                        if (inputBrand == "")
                        {
                            AllLaptops.Add(currentLaptop);
                        }
                        else
                        {
                            if(currentLaptop.Brand == inputBrand)
                            {
                                AllLaptops.Add(currentLaptop);
                            }
                        }
                    }
                    catch
                    {
                        continue;
                    }
                }

                tmp = new List<Laptop>(AllLaptops);

                foreach(var laptop in tmp)
                {
                    if(inputProcessor == "")
                    {
                        break;
                    }
                    else
                    {
                        if(laptop.Processor != inputProcessor)
                        {
                            AllLaptops.Remove(laptop);
                        }
                    }
                }

                tmp = new List<Laptop>(AllLaptops);

                foreach (var laptop in tmp)
                {
                    if (inputResolution == "")
                    {
                        break;
                    }
                    else
                    {
                        if (laptop.Resolution != inputResolution)
                        {
                            AllLaptops.Remove(laptop);
                        }
                    }
                }

                tmp = new List<Laptop>(AllLaptops);

                foreach (var laptop in tmp)
                {
                    if (inputKeyboardType == "")
                    {
                        break;
                    }
                    else
                    {
                        if (laptop.KeyboardType != inputKeyboardType)
                        {
                            AllLaptops.Remove(laptop);
                        }
                    }
                }


                foreach(var laptop in AllLaptops)
                {
                    if(laptop.Price <= inputPrice && laptop.RAM <= inputRAM && laptop.ROM <= inputROM && laptop.BatteryCapacity <= inputBatteryCapacity && laptop.ScreenSize <= inputScreenSize)
                    {
                        SelectedLaptops.Add(laptop);
                    }
                }

                ShowSelectedIems ssi = new ShowSelectedIems(SelectedLaptops, "Laptop");
                ssi.Show();
            }
            else if(ChoosenType == "Desktop")
            {

            }
            else
            {
                // Tablet


            }
        }
    }
}
