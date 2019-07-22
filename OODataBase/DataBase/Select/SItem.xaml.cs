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

namespace DataBase.Select
{
    /// <summary>
    /// Interaction logic for SItem.xaml
    /// </summary>
    public partial class SItem : Window
    {
        private DBManager DB;
        private List<object> AllItemsInDB;
        private List<object> SelectedItems;

        private int inputPrice;
        private string inputBrand = "";

        private Item currentItem;

        public SItem(DBManager db)
        {
            DB = db;
            AllItemsInDB = DB.GetAllItems();

            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();

            SelectedItems = new List<object>();

            try
            {
                inputPrice = Convert.ToInt32(price.Text);
            }
            catch
            {
                inputPrice = Int32.MaxValue;
            }

            inputBrand = brand.Text;

            if(inputBrand == "")
            {
                foreach (var item in AllItemsInDB)
                {
                    currentItem = (Item)item;

                    if (currentItem.Price <= inputPrice)
                    {
                        SelectedItems.Add(currentItem);
                    }
                }
            }
            else
            {
                foreach (var item in AllItemsInDB)
                {
                    currentItem = (Item)item;

                    if (currentItem.Price <= inputPrice && currentItem.Brand == inputBrand)
                    {
                        SelectedItems.Add(currentItem);
                    }
                }
            }

            /*
            if (inputPrice == 0 && inputBrand == "")
            {
                // oba polja prazna 

                SelectedItems = AllItemsInDB;
            }
            else if (inputPrice != 0 && inputBrand == "")
            {
                // samo price

                foreach (var item in AllItemsInDB)
                {
                    currentItem = (Item)item;

                    if (currentItem.Price <= inputPrice)
                    {
                        SelectedItems.Add(currentItem);
                    }
                }
            }
            else if (inputPrice == 0 && inputBrand != "")
            {
                // samo brand

                foreach (var item in AllItemsInDB)
                {
                    currentItem = (Item)item;

                    if (currentItem.Brand == inputBrand)
                    {
                        SelectedItems.Add(currentItem);
                    }
                }
            }
            else
            {
                // oba popunjena

                foreach (var item in AllItemsInDB)
                {
                    currentItem = (Item)item;

                    if (currentItem.Price <= inputPrice && currentItem.Brand == inputBrand)
                    {
                        SelectedItems.Add(currentItem);
                    }
                }
            }
            */

            ShowSelectedIems ssi = new ShowSelectedIems(SelectedItems, "Item");
            ssi.Show();
        }
    }
}
