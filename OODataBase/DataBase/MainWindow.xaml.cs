﻿using DataBase.AddItems;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml;
using System.Xml.Schema;

namespace DataBase
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static Dictionary<string, Dictionary<int, object>> Tables;
        public List<string> Items { get; set; }

        public static Dictionary<string, List<string>> ParentChildren;

        public DBManager db = new DBManager();

        public MainWindow()
        {
            //Items = new List<string>(Tables.Keys);
            DataContext = this;
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //string selected = comboBox_Items.SelectedItem.ToString();

            //Type t = Type.GetType("DataBase.AddItems.Add" + selected);
            //var addLaptop = (Window)Activator.CreateInstance(t, Tables);
            //addLaptop.Show();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            // prosledi SelectItem-u liste area, category, option

            //SelectItems selectItems = new SelectItems(Tables);
            //selectItems.Show();
        }
    }
}
