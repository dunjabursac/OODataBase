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

namespace DataBase.UpdateItems
{
    /// <summary>
    /// Interaction logic for UItem.xaml
    /// </summary>
    public partial class UItem : Window
    {
        DBManager DB;
        public List<string> Items { get; set; }
        public UItem(DBManager db)
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
                    object obj = null;

                    if (version.Text == "")
                    {
                        obj = DB.Read(comboBox_Items.SelectedItem.ToString(), Convert.ToInt32(id.Text), Int32.MaxValue);
                    }
                    else
                    {
                        if (!Int32.TryParse(version.Text, out int tmp1))
                        {
                            MessageBoxResult result = MessageBox.Show("Version must be a number!",
                                                  "Information",
                                                  MessageBoxButton.OK,
                                                  MessageBoxImage.Error);
                        }
                        else
                        {
                            obj = DB.Read(comboBox_Items.SelectedItem.ToString(), Convert.ToInt32(id.Text), Convert.ToInt32(version.Text));
                        }
                    }
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
                        string selected = comboBox_Items.SelectedItem.ToString();

                        Type t = Type.GetType("DataBase." + selected).BaseType;
                        Type t2 = Type.GetType("DataBase.UpdateItems.U" + t.Name);
                        var updateItem = (Window)Activator.CreateInstance(t2, DB, obj, comboBox_Items.SelectedItem.ToString(), Convert.ToInt32(id.Text));
                        updateItem.Show();
                    }
                }
            }
        }
    }
}
