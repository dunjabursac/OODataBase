﻿using DataBase.Select;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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

namespace DataBase
{
    /// <summary>
    /// Interaction logic for SelectItems.xaml
    /// </summary>
    public partial class SelectItems : Window, INotifyPropertyChanged
    {
        public static DBManager DB;
        public static Dictionary<string, List<string>> ParentChildren;

        public event PropertyChangedEventHandler PropertyChanged;

        public List<string> Areas { get; set; }
        public ObservableCollection<string> Categories { get; set; }
        public ObservableCollection<string> Options { get; set; }

        public string selectedArea = "";
        public string selectedCategory = "";



        public SelectItems(DBManager db)
        {
            DB = db;
            ParentChildren = DB.GetParentChildren();
            
            Areas = ParentChildren["Item"];

            DataContext = this;
            InitializeComponent();
        }

        private void Area_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // popunjavas Categories
            
            selectedArea = area.SelectedValue.ToString();
            Categories = new ObservableCollection<string>(ParentChildren[selectedArea]);
            NotifyPropertyChanged("Categories");
        }

        private void Category_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // popunjavas Options

            try
            {
                selectedCategory = category.SelectedValue.ToString();
                Options = new ObservableCollection<string>(ParentChildren[selectedCategory]);
                NotifyPropertyChanged("Options");
            }
            catch
            {
                Options = new ObservableCollection<string>();
                NotifyPropertyChanged("Options");
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();

            if(area.SelectedValue == null)
            {
                // nothing choosen -> Item

                SItem sItem = new SItem(DB);
                sItem.Show();
            }
            else
            {
                if (category.SelectedValue == null)
                {
                    // Area

                    SArea sArea = null;

                    switch (area.SelectedValue.ToString())
                    {
                        case "Computer":
                            sArea = new SArea(DB, "Computer");
                            sArea.Show();
                            break;
                        case "Appliances":
                            sArea = new SArea(DB, "Appliances");
                            sArea.Show();
                            break;
                        case "Phone":
                            sArea = new SArea(DB, "Appliances");
                            sArea.Show();
                            break;
                        default:
                            break;
                    }
                }
                else
                {
                    if (option.SelectedValue == null)
                    {
                        // Area & Category

                        SComputer sComputer = null;
                        SAppliances sAppliances = null;
                        SPhone sPhone = null;

                        switch(category.SelectedValue.ToString())
                        {
                            case "Laptop":
                                sComputer = new SComputer(DB, "Laptop");
                                sComputer.Show();
                                break;
                            case "Desktop":
                                sComputer = new SComputer(DB, "Desktop");
                                sComputer.Show();
                                break;
                            case "Tablet":
                                sComputer = new SComputer(DB, "Tablet");
                                sComputer.Show();
                                break;

                            case "CoolingDevice":
                                sAppliances = new SAppliances(DB, "CoolingDevice");
                                sAppliances.Show();
                                break;
                            case "Machine":
                                sAppliances = new SAppliances(DB, "Machine");
                                sAppliances.Show();
                                break;
                            case "Cooking":
                                sAppliances = new SAppliances(DB, "Cooking");
                                sAppliances.Show();
                                break;

                            case "Landline":
                                sPhone = new SPhone(DB, "Landline");
                                sPhone.Show();
                                break;
                            case "Mobile":
                                sPhone = new SPhone(DB, "Mobile");
                                sPhone.Show();
                                break;

                            default:
                                break;
                        }
                    }
                    else
                    {
                        // Area & Category & Option

                        SCoolingDevice sCoolingDevice = null;
                        SMachine sMachine = null;
                        SCooking sCooking = null;

                        SLandline sLandline = null;
                        SMobile sMobile = null;


                        switch(option.SelectedValue.ToString())
                        {
                            case "AirConditioner":
                                sCoolingDevice = new SCoolingDevice(DB, "AirConditioner");
                                sCoolingDevice.Show();
                                break;
                            case "Fridge":
                                sCoolingDevice = new SCoolingDevice(DB, "Fridge");
                                sCoolingDevice.Show();
                                break;
                            case "Freezer":
                                sCoolingDevice = new SCoolingDevice(DB, "Freezer");
                                sCoolingDevice.Show();
                                break;

                            case "WashingMachine":
                                sMachine = new SMachine(DB, "WashingMachine");
                                sMachine.Show();
                                break;
                            case "DryingMachine":
                                sMachine = new SMachine(DB, "DryingMachine");
                                sMachine.Show();
                                break;
                            case "Dishwasher":
                                sMachine = new SMachine(DB, "Dishwasher");
                                sMachine.Show();
                                break;

                            case "Cooker":
                                sCooking = new SCooking(DB, "Cooker");
                                sCooking.Show();
                                break;
                            case "Oven":
                                sCooking = new SCooking(DB, "Oven");
                                sCooking.Show();
                                break;
                            case "Microwave":
                                sCooking = new SCooking(DB, "Microwave");
                                sCooking.Show();
                                break;


                            case "Wireless":
                                sLandline = new SLandline(DB, "Wireless");
                                sLandline.Show();
                                break;
                            case "Wire":
                                sLandline = new SLandline(DB, "Wire");
                                sLandline.Show();
                                break;

                            case "Smart":
                                sMobile = new SMobile(DB, "Smart");
                                sMobile.Show();
                                break;
                            case "Regular":
                                sMobile = new SMobile(DB, "Regular");
                                sMobile.Show();
                                break;


                            default:
                                break;
                        }
                    }
                }
            }
        }

        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
