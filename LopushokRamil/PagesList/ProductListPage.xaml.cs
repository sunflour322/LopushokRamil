using LopushokRamil.DB;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace LopushokRamil.PagesList
{
    /// <summary>
    /// Логика взаимодействия для ProductListPage.xaml
    /// </summary>
    public partial class ProductListPage : Page
    {
        public int pageNumber = 1;
        public int countPages = 0;
        public int result;
        public ProductListPage()
        {
            InitializeComponent();
            pageNumberTb.Text = pageNumber.ToString();
            var type = App.db.TypeProduct.ToList();
            type.Insert(0, new DB.TypeProduct() { ID = 0, Name = "Все" });
            TypeCb.ItemsSource = type.ToList();
            TypeCb.DisplayMemberPath = "Name";
            Sort();
            TypeCb.SelectedIndex = 0;
            if (App.db.Product.Count() % 20 == 0)
             {
                countPages = App.db.Product.Count() / 20;
             }
            else
            {
                countPages = App.db.Product.Count() / 20 + 1;
            }
            
        }

        private void SearchTb_TextChanged(object sender, TextChangedEventArgs e)
        {
            Sort();

        }
        private void LeftClick(object sender, RoutedEventArgs e)
        {
            pageNumber --;
            if (pageNumber < 1)
            {
                MessageBox.Show("Начало списка");
                pageNumber ++;
                return;
            }
            pageNumberTb.Text = pageNumber.ToString();
            result -= 20;
            Sort();
        }
        private void RightClick(object sender, RoutedEventArgs e)
        {
            pageNumber++;
            if (pageNumber > countPages)
            {
                MessageBox.Show("Конец списка");
                pageNumber--;
                return;
            }
            pageNumberTb.Text = pageNumber.ToString();
            result += 20;
            Sort();
        }

        private void ProductLv_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
        public void Sort()
        {

            var prod = App.db.Product.Include("ProductMaterial.Material").OrderBy(x => x.ID).Skip(result).Take(20).Where(i =>
               i.Name.StartsWith(SearchTb.Text))
               .ToList();
            if (TypeCb.SelectedIndex != 0)
            {
                prod = prod.Where(i => i.ID_type == TypeCb.SelectedIndex).ToList();
            }
            ProductLv.ItemsSource = new List<Product>(prod);
        }

        private void TypeCb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Sort();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void DeleteBtn_Click(object sender, RoutedEventArgs e)
        {
                
        }
    }
}
