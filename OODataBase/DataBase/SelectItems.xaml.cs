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
    /// Interaction logic for SelectItems.xaml
    /// </summary>
    public partial class SelectItems : Window, INotifyPropertyChanged
    {
        public static DBManager DB;
        public static Dictionary<string, List<string>> ParentChildren;

        public event PropertyChangedEventHandler PropertyChanged;

        public List<string> Areas { get; set; }
        public ObservableCollection<string> Categories { get; set; }
        public ObservableCollection<string> Options { get; set; }

        public string selectedArea = "";
        public string selectedCategory = "";



        public SelectItems(DBManager db)
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

        }

        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
