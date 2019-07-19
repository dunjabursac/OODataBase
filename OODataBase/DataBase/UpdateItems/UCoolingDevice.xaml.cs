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
    /// Interaction logic for UCoolingDevice.xaml
    /// </summary>
    public partial class UCoolingDevice : Window
    {
        DBManager DB;
        int ID;
        public UCoolingDevice(DBManager db, object obj, string name, int id)
        {
            DB = db;
            ID = id;
            InitializeComponent();
        }
    }
}
