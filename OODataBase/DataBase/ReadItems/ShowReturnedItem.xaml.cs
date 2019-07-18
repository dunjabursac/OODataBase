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

namespace DataBase.ReadItems
{
    /// <summary>
    /// Interaction logic for ShowReturnedItem.xaml
    /// </summary>
    public partial class ShowReturnedItem : Window
    {
        public ShowReturnedItem(object obj, string name)
        {
            InitializeComponent();
            title.Content = name;
            foreach (PropertyDescriptor descriptor in TypeDescriptor.GetProperties(obj))
            {
                item.Text += descriptor.Name + " : " + descriptor.GetValue(obj) + "\n";
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
