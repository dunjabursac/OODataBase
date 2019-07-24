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
    /// Interaction logic for UMultipleItems.xaml
    /// </summary>
    public partial class UMultipleItems : Window
    {
        DBManager DB;
        public List<string> Items { get; set; }

        List<string> names;
        List<int> ids;
        List<object> objects;

        public UMultipleItems(DBManager db)
        {
            DB = db;
            Items = DB.GetLeavesName();
            DataContext = this;
            names = new List<string>();
            ids = new List<int>();
            objects = new List<object>();
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (DB.UpdateMultiple(names, ids, objects))
            {
                MessageBoxResult result = MessageBox.Show("Updated successfully",
                                                      "Information",
                                                      MessageBoxButton.OK,
                                                      MessageBoxImage.Information);
                if (result == MessageBoxResult.OK)
                {
                    this.Close();
                }
            }
            else
            {
                MessageBoxResult result = MessageBox.Show("Unable to update items!",
                                                      "Information",
                                                      MessageBoxButton.OK,
                                                      MessageBoxImage.Error);
                if (result == MessageBoxResult.OK)
                {
                    this.Close();
                }
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
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
                        var updateItem = (Window)Activator.CreateInstance(t2, DB, obj, comboBox_Items.SelectedItem.ToString(), Convert.ToInt32(id.Text), this);
                        updateItem.Show();
                    }
                }
            }
        }

        public void Updated(object obj)
        {
            objects.Add(obj);
            names.Add(comboBox_Items.SelectedItem.ToString());
            ids.Add(Convert.ToInt32(id.Text));
        }
    }
}
