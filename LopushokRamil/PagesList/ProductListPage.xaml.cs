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
using LopushokRamil.Windows;

namespace LopushokRamil.PagesList
{
    /// <summary>
    /// Логика взаимодействия для ProductListPage.xaml
    /// </summary>
    public partial class ProductListPage : Page
    {
        private int _pageNumber = 1;
        private int _countPages = 0;
        private int _result;
        private Product _product;
        public ProductListPage()
        {
            InitializeComponent();
            pageNumberTb.Text = _pageNumber.ToString();
            var type = App.db.TypeProduct.ToList();
            type.Insert(0, new DB.TypeProduct() { ID = 0, Name = "Все" });
            TypeCb.ItemsSource = type.ToList();
            TypeCb.DisplayMemberPath = "Name";
            Sort();
            TypeCb.SelectedIndex = 0;
            if (App.db.Product.Count() % 20 == 0)
             {
                _countPages = App.db.Product.Count() / 20;
             }
            else
            {
                _countPages = App.db.Product.Count() / 20 + 1;
            }
            
        }

        private void SearchTb_TextChanged(object sender, TextChangedEventArgs e)
        {
            Sort();

        }
        private void LeftClick(object sender, RoutedEventArgs e)
        {
            _pageNumber --;
            if (_pageNumber < 1)
            {
                MessageBox.Show("Начало списка");
                _pageNumber ++;
                return;
            }
            pageNumberTb.Text = _pageNumber.ToString();
            _result -= 20;
            Sort();
        }
        private void RightClick(object sender, RoutedEventArgs e)
        {
            _pageNumber++;
            if (_pageNumber > _countPages)
            {
                MessageBox.Show("Конец списка");
                _pageNumber--;
                return;
            }
            pageNumberTb.Text = _pageNumber.ToString();
            _result += 20;
            Sort();
        }

        private void ProductLv_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
        public void Sort()
        {

            var prod = App.db.Product.Include("ProductMaterial.Material").OrderBy(x => x.ID).Skip(_result).Take(20).Where(i =>
               i.Name.StartsWith(SearchTb.Text) && i.IsActive == true)
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

        private void DeleteBtn_Click(object sender, RoutedEventArgs e)
        {
            _product = (Product)ProductLv.SelectedItem;
            _product.IsActive = false;
            App.db.SaveChanges();
            Sort();
        }

        private void AddBtn_Click(object sender, RoutedEventArgs e)
        {
            AddOrEditProductWindow addOrEditProductWindow = new AddOrEditProductWindow();
            addOrEditProductWindow.ShowDialog();
        }
    }
}
