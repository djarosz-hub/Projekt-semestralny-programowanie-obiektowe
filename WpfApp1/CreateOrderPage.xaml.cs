using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
using ClassLibrary1;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for CreateOrderPage.xaml
    /// Page enables to create order according to the pattern, at least one product must be added, employee and client is necessary to finalize order creation process.
    /// </summary>
    public partial class CreateOrderPage : Page
    {
        MydbEntities db;
        string mostRecentClientName = "";
        string mostRecentEmployeeName = "";
        string mostRecentProductText = "";
        List<DetailedProductOrder> productsInOrder = new List<DetailedProductOrder>();
        public CreateOrderPage(MydbEntities db)
        {
            InitializeComponent();
            this.db = db;
            FillAllClients();
            FillAllEmployees();
            FillProductsDG();
        }
        void Page_Loaded(object sender, RoutedEventArgs e)
        {
            ResetAllValues();
        }
        void ResetAllValues()
        {
            ProductNameTB.Text = "";
            TotalPriceTB.Text = "";
            ClientNameTB.Text = "";
            EmployeeNameTB.Text = "";
            productsInOrder.Clear();
            RefreshProductsInOrderDG();
            ClientsDG.SelectedIndex = -1;
            EmployeesDG.SelectedIndex = -1;
        }
        void CreateNewOrder_Click(object sender, RoutedEventArgs e)
        {
            var clientDGSelection = ClientsDG.SelectedItem;
            if (clientDGSelection == null)
            {
                MessageBox.Show($"Select client for order.");
                return;
            }
            var employeeDGSelection = EmployeesDG.SelectedItem;
            if (employeeDGSelection == null)
            {
                MessageBox.Show($"Select employee for order.");
                return;
            }
            if (productsInOrder.Count() == 0)
            {
                MessageBox.Show($"Order must contain at least one product.");
                return;
            }

            Orders newOrder = new Orders();
            newOrder.client_id = ((Clients)clientDGSelection).client_id;
            newOrder.order_employee = ((Employees)employeeDGSelection).employee_id;
            var date = DateTime.Now;
            TimeSpan ts = new TimeSpan(0, 1, 0);
            date -= ts;
            ///one minute is substracted - when DateTime is converted to sql ShortDateTime the value is rounded up, it may breake sql constraint <= GETDATE()///
            
            newOrder.order_date = date;
            foreach (var p in productsInOrder)
            {
                Ord_Prod newOrderProduct = new Ord_Prod();
                newOrderProduct.product_id = p.productId;
                newOrderProduct.quantity = (short)p.quantity;
                newOrder.Ord_Prod.Add(newOrderProduct);
            }
            try
            {
                db.Orders.Add(newOrder);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
            MessageBox.Show("New order successfully created.");
            ResetAllValues();

        }
        private void AddToOrder_Click(object sender, RoutedEventArgs e)
        {
            var selected = ProductsDG.SelectedItem;
            if (selected == null)
            {
                MessageBox.Show("Select product to add.");
                return;
            }
            var product = ((DetailedProductOrder)selected);
            productsInOrder.Add(new DetailedProductOrder { productId = product.productId, productName = product.productName, productCategory = product.productCategory, producer = product.producer, price = product.price, quantity = 1 });
            RefreshProductsInOrderDG();
            FillProductsDG();
        }
        void RefreshProductsInOrderDG()
        {
            ProductsInOrderDG.Items.Clear();
            foreach (var p in productsInOrder)
            {
                p.sum = p.price * p.quantity;
                ProductsInOrderDG.Items.Add(p);
            }
            RefreshTotalPrice();
        }
        void RefreshTotalPrice()
        {
            TotalPriceTB.Text = "";
            decimal totalPrice = 0;
            foreach (var p in productsInOrder)
                totalPrice += p.price * p.quantity;
            TotalPriceTB.Text = $"{totalPrice}";
        }
        void FillAllClients()
        {
            ClientsDG.Items.Clear();
            foreach (var c in db.Clients)
                ClientsDG.Items.Add(c);
        }
        void FillAllEmployees()
        {
            EmployeesDG.Items.Clear();
            foreach (var emp in db.Employees)
                EmployeesDG.Items.Add(emp);
        }
        void FillProductsDG()
        {
            ProductsDG.Items.Clear();
            foreach (var p in db.Products)
            {
                if (productsInOrder.Any(x => x.productId == p.product_id))
                    continue;
                string categoryName = db.Categories.Single(x => x.category_id == p.category).category_name;
                string producerName = db.Producers.Single(x => x.producer_id == p.producer).producer_name;
                ProductsDG.Items.Add(new DetailedProductOrder { productId = p.product_id, productName = p.product_name, productCategory = categoryName, producer = producerName, price = p.price });
            }

        }
        async void Client_textChanged(object sender, TextChangedEventArgs e)
        {
            string enteredText = ClientNameTB.Text;
            mostRecentClientName = enteredText;
            if (ClientNameTB.Text == "")
            {
                FillAllClients();
                return;
            }
            await Task.Delay(500);

            if (enteredText == mostRecentClientName)
            {
                ClientsDG.Items.Clear();
                var filteredClients = db.Clients.Where(x => x.client_name.Contains(mostRecentClientName));
                foreach (var c in filteredClients)
                    ClientsDG.Items.Add(c);
            }
        }
        async void Employee_textChanged(object sender, TextChangedEventArgs e)
        {
            string enteredText = EmployeeNameTB.Text;
            mostRecentEmployeeName = enteredText;
            if (EmployeeNameTB.Text == "")
            {
                FillAllEmployees();
                return;
            }
            await Task.Delay(500);

            if (enteredText == mostRecentEmployeeName)
            {
                EmployeesDG.Items.Clear();
                var filteredEmployees = db.Employees.Where(x => x.employee_name.Contains(mostRecentEmployeeName));
                foreach (var emp in filteredEmployees)
                    EmployeesDG.Items.Add(emp);
            }
        }
        async void Product_textChanged(object sender, TextChangedEventArgs e)
        {
            string enteredText = ProductNameTB.Text;
            mostRecentProductText = enteredText;
            if (ProductNameTB.Text == "")
            {
                FillProductsDG();
                return;
            }
            await Task.Delay(500);

            if (enteredText == mostRecentProductText)
            {
                ProductsDG.Items.Clear();
                var filteredProducts = db.Products.Where(x => x.product_name.Contains(mostRecentProductText));
                foreach (var p in filteredProducts)
                {
                    string categoryName = db.Categories.Single(x => x.category_id == p.category).category_name;
                    string producerName = db.Producers.Single(x => x.producer_id == p.producer).producer_name;
                    ProductsDG.Items.Add(new DetailedProductOrder { productId = p.product_id, productName = p.product_name, productCategory = categoryName, producer = producerName, price = p.price });
                }
            }
        }
        void SubstractOne_Click(object sender, RoutedEventArgs e)
        {
            var selected = ProductsInOrderDG.SelectedItem;
            if (selected == null)
            {
                MessageBox.Show("Select product first.");
                return;
            }
            DetailedProductOrder selectedProduct = (DetailedProductOrder)selected;
            selectedProduct.quantity--;
            if (selectedProduct.quantity == 0)
            {
                productsInOrder.Remove(selectedProduct);
                FillProductsDG();
            }
            RefreshProductsInOrderDG();
        }
        void AddOne_Click(object sender, RoutedEventArgs e)
        {
            var selected = ProductsInOrderDG.SelectedItem;
            if (selected == null)
            {
                MessageBox.Show("Select product first.");
                return;
            }
            DetailedProductOrder selectedProduct = (DetailedProductOrder)selected;
            if (selectedProduct.quantity == Int16.MaxValue)
            {
                MessageBox.Show($"Maximum amount of product in single order.");
                return;
            }
            selectedProduct.quantity++;
            RefreshProductsInOrderDG();
        }
    }
}
