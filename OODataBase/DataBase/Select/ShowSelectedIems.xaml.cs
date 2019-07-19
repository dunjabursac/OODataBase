using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace DataBase.Select
{
    /// <summary>
    /// Interaction logic for ShowSelectedIems.xaml
    /// </summary>
    public partial class ShowSelectedIems : Window
    {
        public ShowSelectedIems(List<object> selectedItems, string selectedType)
        {
            InitializeComponent();

            title.Content = "Selected " + selectedType;

            // Jovanov ispis SelectedItems - a

            foreach (object obj in selectedItems)
            {
                foreach (PropertyDescriptor descriptor in TypeDescriptor.GetProperties(obj))
                {
                    item.Text += descriptor.Name + " : " + descriptor.GetValue(obj) + "\n";
                }

                item.Text += "\n\n";
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
