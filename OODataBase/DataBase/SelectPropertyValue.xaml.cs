using DataBase.Select;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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
    /// Interaction logic for SelectPropertyValue.xaml
    /// </summary>
    public partial class SelectPropertyValue : Window, INotifyPropertyChanged
    {
        public static DBManager DB;
        public static Dictionary<string, List<string>> ParentChildren;

        public event PropertyChangedEventHandler PropertyChanged;

        public List<string> Areas { get; set; }
        public ObservableCollection<string> Categories { get; set; }
        public ObservableCollection<string> Options { get; set; }
        public ObservableCollection<string> Properties { get; set; }

        public string selectedArea = "";
        public string selectedCategory = "";
        public string selectedProperty = "";

        public string ChoosenType { get; set; }


        public SelectPropertyValue(DBManager db)
        {
            DB = db;
            ParentChildren = DB.GetParentChildren();

            Areas = ParentChildren["Item"];

            DataContext = this;
            InitializeComponent();
        }

        private void Area_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // popunjavas Categories

            selectedArea = area.SelectedValue.ToString();
            Categories = new ObservableCollection<string>(ParentChildren[selectedArea]);
            NotifyPropertyChanged("Categories");
        }

        private void Category_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // popunjavas Options

            try
            {
                selectedCategory = category.SelectedValue.ToString();
                Options = new ObservableCollection<string>(ParentChildren[selectedCategory]);
                NotifyPropertyChanged("Options");
            }
            catch
            {
                Options = new ObservableCollection<string>();
                NotifyPropertyChanged("Options");
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // Choose
            // popunjavas Properties

            property.SelectedValue = null;
            value.Text = "";
            version.Text = "";
            
            if(area.Text == "")
            {
                ChoosenType = "Item";
            }
            else
            {
                if(category.Text == "")
                {
                    ChoosenType = area.Text;
                }
                else
                {
                    if(option.Text == "")
                    {
                        ChoosenType = category.Text;
                    }
                    else
                    {
                        ChoosenType = option.Text;
                    }
                }
            }


            Properties = new ObservableCollection<string>();

            Type type = Type.GetType($"DataBase.{ChoosenType}");
            var currentClassInstance = Activator.CreateInstance(type);


            foreach (PropertyDescriptor descriptor in TypeDescriptor.GetProperties(currentClassInstance))
            {
                Properties.Add(descriptor.Name);
            }

            NotifyPropertyChanged("Properties");
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            // Select

            int inputVersion;

            try
            {
                inputVersion = Convert.ToInt32(version.Text);
            }
            catch
            {
                inputVersion = Int32.MaxValue;
            }

            if (value.Text == "" && property.SelectedValue != null)
            {
                MessageBoxResult result = MessageBox.Show("Value is required!",
                                      "Information",
                                      MessageBoxButton.OK,
                                      MessageBoxImage.Error);
            }
            else
            {
                ShowSelectedIems ssi = new ShowSelectedIems(DB.Select(ChoosenType, property.Text, value.Text, inputVersion), ChoosenType);
                ssi.Show();
            }
        }

        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
