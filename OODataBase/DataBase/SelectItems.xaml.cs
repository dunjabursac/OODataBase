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

namespace DataBase
{
    /// <summary>
    /// Interaction logic for SelectItems.xaml
    /// </summary>
    public partial class SelectItems : Window
    {
        public static Dictionary<string, Dictionary<int, object>> Tables;

        public SelectItems(Dictionary<string, Dictionary<int, object>> tables)
        {
            Tables = tables;
            InitializeComponent();
        }

        private void Area_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // popunjavas Categories
        }

        private void Category_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
