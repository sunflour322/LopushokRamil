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
using LopushokRamil.DB;
using LopushokRamil.PagesList;

namespace LopushokRamil.Windows
{
    /// <summary>
    /// Логика взаимодействия для AddOrEditProductWindow.xaml
    /// </summary>
    public partial class AddOrEditProductWindow : Window
    {
        public event Action DataUpdated;

        private Product prod;

        public AddOrEditProductWindow()
        {
            InitializeComponent();
            TypeCb.ItemsSource = App.db.TypeProduct.ToList();
            TypeCb.DisplayMemberPath = "Name";
        }

        public AddOrEditProductWindow(Product product)
        {
            InitializeComponent();
            prod = product;
            TypeCb.ItemsSource = App.db.TypeProduct.ToList();
            TypeCb.DisplayMemberPath = "Name";
            NameTb.Text = prod.Name;
            ArticleTb.Text = prod.Article.ToString();
            CostTb.Text = prod.MinCost.ToString();
            CountPeopleTb.Text = prod.PeopleCount.ToString();
            ShopNumberTb.Text = prod.ShopNumber.ToString();
            TypeCb.SelectedIndex = (int)(prod.ID_type - 1);
        }

        private void AddOrEditBtn(object sender, RoutedEventArgs e)
        {
            try
            {
                if (prod != null)
                {
                    prod.Name = NameTb.Text;
                    prod.Article = int.Parse(ArticleTb.Text);
                    prod.MinCost = int.Parse(CostTb.Text);
                    prod.PeopleCount = int.Parse(CountPeopleTb.Text);
                    prod.ShopNumber = int.Parse(ShopNumberTb.Text);
                    prod.ID_type = TypeCb.SelectedIndex + 1;
                    App.db.SaveChanges();
                    MessageBox.Show("Успешное редактирование");
                }
                else
                {
                    Product product = new Product();
                    product.Name = NameTb.Text;
                    product.Article = int.Parse(ArticleTb.Text);
                    product.MinCost = int.Parse(CostTb.Text);
                    product.PeopleCount = int.Parse(CountPeopleTb.Text);
                    product.ShopNumber = int.Parse(ShopNumberTb.Text);
                    product.ID_type = TypeCb.SelectedIndex + 1;
                    product.IsActive = true;
                    product.Image = "/ProductImages/picture.png";
                    App.db.Product.Add(product);
                    App.db.SaveChanges();
                    MessageBox.Show("Успешное добавление");
                }

                DataUpdated?.Invoke(); 
                Close();
            }
            catch (Exception)
            {
                MessageBox.Show("Ошибка");
            }
        }

        private void CloseBtn(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
