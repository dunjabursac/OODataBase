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

namespace DataBase.ReadItems
{
    /// <summary>
    /// Interaction logic for ReadItem.xaml
    /// </summary>
    public partial class ReadItem : Window
    {
        DBManager DB;
        public List<string> Items { get; set; }
        public ReadItem(DBManager db)
        {
            DB = db;
            Items = DB.GetLeavesName();
            DataContext = this;
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (comboBox_Items.SelectedItem == null || id.Text == "")
            {
                MessageBoxResult result = MessageBox.Show("Type and ID are required!",
                                          "Information",
                                          MessageBoxButton.OK,
                                          MessageBoxImage.Error);
            }
            else
            {
                if (!Int32.TryParse(id.Text, out int tmp))
                {
                    MessageBoxResult result = MessageBox.Show("ID must be a number!",
                                          "Information",
                                          MessageBoxButton.OK,
                                          MessageBoxImage.Error);
                }
                else
                {
                    object obj = DB.Read(comboBox_Items.SelectedItem.ToString(), Convert.ToInt32(id.Text));
                    if (obj == null)
                    {
                        MessageBoxResult result = MessageBox.Show("Unable to read item!",
                                                  "Information",
                                                  MessageBoxButton.OK,
                                                  MessageBoxImage.Error);
                        if (result == MessageBoxResult.OK)
                        {
                            id.Text = "";
                        }
                    }
                    else
                    {
                        ShowReturnedItem sri = new ShowReturnedItem(obj, comboBox_Items.SelectedItem.ToString());
                        sri.Show();
                    }
                }
            }
        }
    }
}
