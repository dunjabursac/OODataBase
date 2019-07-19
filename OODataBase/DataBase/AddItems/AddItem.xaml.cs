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

namespace DataBase.AddItems
{
    /// <summary>
    /// Interaction logic for AddItem.xaml
    /// </summary>
    public partial class AddItem : Window
    {
        DBManager DB;
        public List<string> Items { get; set; }
        public AddItem(DBManager db)
        {
            DB = db;
            Items = DB.GetLeavesName();
            DataContext = this;
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (comboBox_Items.SelectedItem == null)
            {
                MessageBoxResult result = MessageBox.Show("Type is not selected!",
                                          "Information",
                                          MessageBoxButton.OK,
                                          MessageBoxImage.Error);
            }
            else
            {
                string selected = comboBox_Items.SelectedItem.ToString();

                Type t = Type.GetType("DataBase.AddItems.Add" + selected);
                var addItem = (Window)Activator.CreateInstance(t, DB);
                addItem.Show();
            }
        }
    }
}
