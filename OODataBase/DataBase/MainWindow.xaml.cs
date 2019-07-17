using DataBase.AddItems;
using DataBase.DeleteItems;
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
        public List<string> Items { get; set; }

        public DBManager db = new DBManager();

        public MainWindow()
        {
            Items = db.GetLeavesName();
            DataContext = this;
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string selected = comboBox_Items.SelectedItem.ToString();

            Type t = Type.GetType("DataBase.AddItems.Add" + selected);
            var addLaptop = (Window)Activator.CreateInstance(t, db);
            addLaptop.Show();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            // prosledi SelectItem-u liste dictionary ParentChildren



            SelectItems selectItems = new SelectItems(db);
            selectItems.Show();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            DeleteItem di = new DeleteItem(db);
            di.Show();
        }
    }
}
