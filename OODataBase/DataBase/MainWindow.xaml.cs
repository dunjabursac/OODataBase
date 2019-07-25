using DataBase.AddItems;
using DataBase.DeleteItems;
using DataBase.ReadItems;
using DataBase.UpdateItems;
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
        public DBManager db = DBManager.DBM_Instance;

        public MainWindow()
        {
            DataContext = this;
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            AddItem addItem = new AddItem(db);
            addItem.Show();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            SelectItems selectItems = new SelectItems(db);
            selectItems.Show();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            DeleteItem di = new DeleteItem(db);
            di.Show();
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            ReadItem ri = new ReadItem(db);
            ri.Show();
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            UItem ui = new UItem(db);
            ui.Show();
        }

        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            SelectPropertyValue selectPropertyValue = new SelectPropertyValue(db);
            selectPropertyValue.Show();
        }

        private void Button_Click_6(object sender, RoutedEventArgs e)
        {
            UMultipleItems ui = new UMultipleItems(db);
            ui.Show();
        }
    }
}
