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

namespace DataBase.DeleteItems
{
    /// <summary>
    /// Interaction logic for DeleteItem.xaml
    /// </summary>
    public partial class DeleteItem : Window
    {
        DBManager DB;
        public List<string> Items { get; set; }
        public DeleteItem(DBManager db)
        {
            DB = db;
            Items = DB.GetLeavesName();
            DataContext = this;
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (DB.Delete(comboBox_Items.SelectedItem.ToString(), Convert.ToInt32(id.Text)))
            {
                MessageBoxResult result = MessageBox.Show("Deleted successfully",
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
                MessageBoxResult result = MessageBox.Show("Unable to delete item!",
                                          "Information",
                                          MessageBoxButton.OK,
                                          MessageBoxImage.Error);
                if (result == MessageBoxResult.OK)
                {
                    id.Text = "";
                }
            }
        }
    }
}
