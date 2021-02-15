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
using ClassLibrary1;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for ProductsPage.xaml
    /// Page which handles product viewing, creating and filtering by assigned category.
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
            if (category_nameComboBox.SelectedItem == null)
            {
                MessageBox.Show($"Empty category.");
                return;
            }
            RefillProductsByCatList();
        }
        private void RefillProductsByCatList()
        {
            catList.Clear();
            ItemsInCategoryDG.Items.Clear();
            var categoryId = (category_nameComboBox.SelectedItem as Categories).category_id;
            var products = db.Products.Where(x => x.category == categoryId);
            foreach (var p in products)
            {
                string producerName = db.Producers.Single(x => x.producer_id == p.producer).producer_name;
                catList.Add(new ItemsForCatGrid { id = p.product_id, product_name = p.product_name, producer = producerName, price = p.price });
            }
            if(catList.Count() == 0)
            {
                MessageBox.Show($"No products assigned to this category.");
                return;
            }
            foreach (var i in catList)
                ItemsInCategoryDG.Items.Add(i);
        }
        private void Add_Click(object sender, RoutedEventArgs e)
        {
            var cat = db.Categories.SingleOrDefault(x => x.category_name == AddCategoriesCB.Text);
            var prod = db.Producers.SingleOrDefault(x => x.producer_name == AddProducersCB.Text);
            if (cat == null || prod == null)
            {
                MessageBox.Show("Choose correct category and producer");
                return;
            }
            if (commander.IsInputInvalid(AddNameTB.Text, "Product name"))
            {
                MessageBox.Show($"Product name must not be empty");
                return;
            }
            string integerPartS = AddPriceIntTB.Text.Trim();
            string decimalPartS = AddatePriceDecimalTB.Text.Trim();
            if (!int.TryParse(integerPartS, out int integerPart) || !int.TryParse(decimalPartS, out int decimalPart))
            {
                MessageBox.Show($"Invalid value of price.");
                return;
            }
            string concatPrice = integerPartS + "." + decimalPartS;
            if (!decimal.TryParse(concatPrice, out decimal newPrice))
            {
                MessageBox.Show($"Invalid price");
                return;
            }
            string productName = AddNameTB.Text;
            int categoryId = cat.category_id;
            int producerId = prod.producer_id;
            if (commander.ExistsInDatabaseByNameCaseSensitive(OSHome.DbSources.Products, productName))
            {
                Products existingProduct = db.Products.Single(x => x.product_name == productName);
                if (existingProduct.price == newPrice && existingProduct.category == categoryId && existingProduct.producer == producerId)
                {
                    MessageBox.Show($"Product with values:\nName: {productName} already exists. (ID = {existingProduct.product_id})");
                    return;
                }
            }
            Products newProduct = new Products();
            newProduct.product_name = productName;
            newProduct.category = (short)categoryId;
            newProduct.producer = (short)producerId;
            newProduct.price = newPrice;
            db.Products.Add(newProduct);
            db.SaveChanges();
            if (category_nameComboBox.Text != string.Empty && newProduct.category == (category_nameComboBox.SelectedItem as Categories).category_id)
                RefillProductsByCatList();
            MessageBox.Show($"Successfully created new product:\nID: {newProduct.product_name}");
            AddCategoriesCB.SelectedIndex = 0;
            AddProducersCB.SelectedIndex = 0;
            AddNameTB.Text = "Product name";
            AddPriceIntTB.Text = "Integer part";
            AddatePriceDecimalTB.Text = "Decimal part";
        }

        private void Remove_Click(object sender, RoutedEventArgs e)
        {
            string stringId = RemoveIDTB.Text.Trim();
            if (commander.IsInputInvalid(stringId, "ID") || !int.TryParse(stringId, out int id))
            {
                MessageBox.Show($"{stringId} is not valid id.");
                return;
            }
            if (!commander.ExistsInDatabaseByID(OSHome.DbSources.Products, id) || commander.IsAssignedToEntity(OSHome.DbSources.Products, id))
            {
                MessageBox.Show($"Product doesn't exist in database or is already assigned to some order.");
                return;
            }
            if (!commander.FinalAcceptancePrompt())
                return;
            commander.RemoveFromDb(OSHome.DbSources.Products, id);
            MessageBox.Show($"Successfully removed from database.");
            RemoveIDTB.Text = "ID";

        }
        private void Update_Click(object sender, RoutedEventArgs e)
        {
            string stringId = UpdateIDTB.Text.Trim();
            if (commander.IsInputInvalid(stringId, "ID") || !int.TryParse(stringId, out int id))
            {
                MessageBox.Show($"{stringId} is not valid id.");
                return;
            }
            if (!commander.ExistsInDatabaseByID(OSHome.DbSources.Products, id))
            {
                MessageBox.Show($"Product doesn't exist in database.");
                return;
            }
            string integerPartS = UpdatePriceIntTB.Text.Trim();
            string decimalPartS = UpdatePriceDecimalTB.Text.Trim();
            if (!int.TryParse(integerPartS, out int integerPart) || !int.TryParse(decimalPartS, out int decimalPart))
            {
                MessageBox.Show($"Invalid value of new price.");
                return;
            }
            string concatPrice = integerPartS + "." + decimalPartS;
            if (!decimal.TryParse(concatPrice, out decimal newPrice))
            {
                MessageBox.Show($"Invalid price");
                return;
            }
            if (!commander.FinalAcceptancePrompt())
                return;
            Products existingProduct = db.Products.Single(x => x.product_id == id);
            if (existingProduct.price == newPrice)
            {
                MessageBox.Show($"New price is same as current price");
                return;
            }
            existingProduct.price = newPrice;
            productsDataGrid.Items.Refresh();
            db.SaveChanges();
            var selectedCats = (Categories)category_nameComboBox.SelectedItem;
            if ( selectedCats != null && existingProduct.category == selectedCats.category_id)
                RefillProductsByCatList();
            MessageBox.Show($"Successfully updated price of: {existingProduct.product_name} into: {newPrice}");
        }
        private void Find_Click(object sender, RoutedEventArgs e)
        {
            string productName = FindNameTB.Text.Trim();
            if (commander.IsInputInvalid(productName, "Product name"))
            {
                MessageBox.Show($"Invalid or empty input.");
                return;
            }
            var products = db.Products.Where(x => x.product_name.ToLower() == productName.ToLower());
            if (products.Count() > 0)
            {
                StringBuilder sb = new StringBuilder();
                foreach(var p in products)
                {
                string category = db.Categories.Single(x => x.category_id == p.category).category_name;
                string producer = db.Producers.Single(x => x.producer_id == p.producer).producer_name;
                    sb.Append($"ID: {p.product_id}\tName: {p.product_name}\tCategory: {category}\tProducer: {producer}\n");
                }
                MessageBox.Show($"Found:\n{sb}");
                return;
            }
            MessageBox.Show($"Not found.");
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
                AddCategoriesCB.Items.Add(item);
            }
        }
        private void FillAvailableProducers()
        {
            foreach (var p in db.Producers)
            {
                ComboBoxItem item = new ComboBoxItem();
                item.Content = p.producer_name;
                AddProducersCB.Items.Add(item);
            }
        }
    }
}
