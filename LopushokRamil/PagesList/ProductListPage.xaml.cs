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
            ProductLv.ItemsSource = App.db.Product
               .Include("ProductMaterial.Material").OrderBy(x => x.ID).Skip(result).Take(20)// Загружаем связанные материалы
               .ToList();
             if(App.db.Product.Count() % 20 == 0)
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
            ProductLv.ItemsSource = App.db.Product.Include("ProductMaterial.Material").OrderBy(x => x.ID).Skip(result).Take(20).Where(i =>
               i.Name.StartsWith(SearchTb.Text))// Загружаем связанные материалы
               .ToList();
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
            ProductLv.ItemsSource = App.db.Product
              .Include("ProductMaterial.Material").OrderBy(x => x.ID).Skip(result).Take(20)// Загружаем связанные материалы
              .ToList();
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
            ProductLv.ItemsSource = App.db.Product
              .Include("ProductMaterial.Material").OrderBy(x => x.ID).Skip(result).Take(20)// Загружаем связанные материалы
              .ToList();
        }

        private void ProductLv_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        
    }
}
