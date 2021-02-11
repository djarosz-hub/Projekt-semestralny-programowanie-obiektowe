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
using System.Data.Entity;
using System.Data;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for ProductsPage.xaml
    /// </summary>
    public partial class ProductsPage : Page
    {
        MydbEntities db;
        CollectionViewSource productViewSource;
        CollectionViewSource catViewSource;
        OSHome commander;
        List<ItemsForCatGrid> catList = new List<ItemsForCatGrid>();
        public ProductsPage(MydbEntities db, OSHome commander)
        {
            InitializeComponent();
            this.db = db;
            this.commander = commander;
            productViewSource = ((CollectionViewSource)(FindResource("productsViewSource")));
            catViewSource = ((CollectionViewSource)(FindResource("categoriesViewSource")));
            DataContext = this;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            db.Products.Load();
            productViewSource.Source = db.Products.Local;
            catViewSource.Source = db.Categories.Local;
            FillAvailableCategories();
            FillAvailableProducers();
        }

        private void Check_Click(object sender, RoutedEventArgs e)
        {
            if(category_nameComboBox.SelectedItem == null)
            {
                MessageBox.Show($"Empty category.");
                return;
            }
            RefillCatList();
        }
        private void RefillCatList()
        {
            catList.Clear();
            ItemsInCategoryDG.Items.Clear();
            var id = category_nameComboBox.SelectedItem as Categories;
            int categoryId = id.category_id;
            var products = db.Products.Where(x => x.category == categoryId);
            foreach (var p in products)
            {
                string producerName = db.Producers.Single(x => x.producer_id == p.producer).producer_name;
                catList.Add(new ItemsForCatGrid { id = p.product_id, product_name = p.product_name, producer = producerName, price = p.price });
            }
            foreach (var i in catList)
                ItemsInCategoryDG.Items.Add(i);
        }
        private void Add_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Remove_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Update_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Find_Click(object sender, RoutedEventArgs e)
        {

        }
        public void RemovePlaceHolder(object sender, EventArgs e)
        {
            string[] basics = new string[] { "ID", "Product name", "Integer part", "Decimal part" };
            TextBox tb = sender as TextBox;
            if (basics.Contains(tb.Text))
            {
                tb.Text = "";
            }
        }
        private void FillAvailableCategories()
        {
            foreach (var c in db.Categories)
            {
                ComboBoxItem item = new ComboBoxItem();
                item.Content = c.category_name;
                CategoriesCB.Items.Add(item);
            }
        }
        private void FillAvailableProducers()
        {
            foreach (var p in db.Producers)
            {
                ComboBoxItem item = new ComboBoxItem();
                item.Content = p.producer_name;
                ProducersCB.Items.Add(item);
            }
        }
        private void FillProductsAssignedToChoosenCategory()
        {

        }
        class ItemsForCatGrid
        {
            public int id { get; set; }
            public string product_name { get; set; }
            public string producer { get; set; }
            public decimal price { get; set; }
        }
    }
}
