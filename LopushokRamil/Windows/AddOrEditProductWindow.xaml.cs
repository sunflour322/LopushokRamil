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
        private List<ProductMaterial> productMaterials;

        public AddOrEditProductWindow()
        {
            InitializeComponent();
            LoadMaterials();
            TypeCb.ItemsSource = App.db.TypeProduct.ToList();
            TypeCb.DisplayMemberPath = "Name";
            MaterialCb.ItemsSource = App.db.Material.ToList();
        }

        public AddOrEditProductWindow(Product product)
        {
            InitializeComponent();
            prod = product;
            LoadMaterials();
            TypeCb.ItemsSource = App.db.TypeProduct.ToList();
            TypeCb.DisplayMemberPath = "Name";
            MaterialCb.ItemsSource = App.db.Material.ToList();
            NameTb.Text = prod.Name;
            ArticleTb.Text = prod.Article.ToString();
            CostTb.Text = prod.MinCost.ToString();
            CountPeopleTb.Text = prod.PeopleCount.ToString();
            ShopNumberTb.Text = prod.ShopNumber.ToString();
            TypeCb.SelectedIndex = (int)(prod.ID_type - 1);

            // Загрузка материалов продукта
            productMaterials = App.db.ProductMaterial.Where(pm => pm.ID_prod == prod.ID).ToList();
            MaterialsGrid.ItemsSource = productMaterials;
        }

        private void LoadMaterials()
        {
            productMaterials = new List<ProductMaterial>();
            MaterialsGrid.ItemsSource = productMaterials;
        }

        private void AddMaterial_Click(object sender, RoutedEventArgs e)
        {
            if (MaterialCb.SelectedItem is Material selectedMaterial && int.TryParse(MaterialCountTb.Text, out int count))
            {
                var existingMaterial = productMaterials.FirstOrDefault(pm => pm.ID_mat == selectedMaterial.ID);
                if (existingMaterial != null)
                {
                    existingMaterial.Count += count;
                }
                else
                {
                    var newProductMaterial = new ProductMaterial
                    {
                        ID_mat = selectedMaterial.ID,
                        ID_prod = prod?.ID ?? 0,
                        Count = count,
                        Material = selectedMaterial
                    };
                    productMaterials.Add(newProductMaterial);
                }
                MaterialsGrid.Items.Refresh();
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите материал и укажите количество.");
            }
        }

        private void DeleteMaterial_Click(object sender, RoutedEventArgs e)
        {
            if (MaterialsGrid.SelectedItem is ProductMaterial selectedMaterial)
            {
                productMaterials.Remove(selectedMaterial);
                MaterialsGrid.Items.Refresh();
            }
        }

        private void AddOrEditBtn(object sender, RoutedEventArgs e)
        {
            try
            {
                if (prod != null)
                {
                    // Обновление данных продукта
                    prod.Name = NameTb.Text;
                    prod.Article = int.Parse(ArticleTb.Text);
                    prod.MinCost = int.Parse(CostTb.Text);
                    prod.PeopleCount = int.Parse(CountPeopleTb.Text);
                    prod.ShopNumber = int.Parse(ShopNumberTb.Text);
                    prod.ID_type = TypeCb.SelectedIndex + 1;

                    // Получаем текущие материалы продукта из базы данных
                    var existingMaterials = App.db.ProductMaterial
                        .Where(pm => pm.ID_prod == prod.ID)
                        .ToList();

                    // Удаляем материалы, которые больше не нужны
                    foreach (var existingMaterial in existingMaterials)
                    {
                        if (!productMaterials.Any(pm => pm.ID_mat == existingMaterial.ID_mat))
                        {
                            App.db.ProductMaterial.Remove(existingMaterial);
                        }
                    }

                    // Добавляем или обновляем материалы
                    foreach (var pm in productMaterials)
                    {
                        var existingMaterial = existingMaterials
                            .FirstOrDefault(em => em.ID_mat == pm.ID_mat);

                        if (existingMaterial != null)
                        {
                            // Обновляем количество материала
                            existingMaterial.Count = pm.Count;
                        }
                        else
                        {
                            // Добавляем новый материал
                            pm.ID_prod = prod.ID;
                            App.db.ProductMaterial.Add(pm);
                        }
                    }
                }
                else
                {
                    // Создание нового продукта
                    Product product = new Product
                    {
                        Name = NameTb.Text,
                        Article = int.Parse(ArticleTb.Text),
                        MinCost = int.Parse(CostTb.Text),
                        PeopleCount = int.Parse(CountPeopleTb.Text),
                        ShopNumber = int.Parse(ShopNumberTb.Text),
                        ID_type = TypeCb.SelectedIndex + 1,
                        IsActive = true,
                        Image = "/ProductImages/picture.png"
                    };
                    App.db.Product.Add(product);
                    App.db.SaveChanges();

                    // Добавление материалов продукта
                    foreach (var pm in productMaterials)
                    {
                        pm.ID_prod = product.ID;
                        App.db.ProductMaterial.Add(pm);
                    }
                }

                // Сохранение изменений в базе данных
                App.db.SaveChanges();
                MessageBox.Show("Успешное сохранение");
                DataUpdated?.Invoke();
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}");
            }
        }

        private void CloseBtn(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
