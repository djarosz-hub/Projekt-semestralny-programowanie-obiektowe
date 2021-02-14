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
using ClassLibrary1;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for OrdersPage.xaml
    /// </summary>
    public partial class OrdersPage : Page
    {
        MydbEntities db;
        CreateOrderPage createOrderPage;
        OSHome commander;
        List<DetailedProductOrder> productsInOrderList = new List<DetailedProductOrder>();
        List<Order> ordersList = new List<Order>();
        public OrdersPage(MydbEntities db, OSHome commander)
        {
            InitializeComponent();
            this.db = db;
            this.commander = commander;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            RefreshOrders();
        }
        private void RefreshOrders()
        {
            ordersList.Clear();
            OrdersDG.Items.Clear();
            foreach (var order in db.Orders)
            {
                string orderDate = order.order_date.ToString("MM/dd/yyyy hh:mm tt");

                var client = db.Clients.Single(x => x.client_id == order.client_id);
                string fullClientName = client.client_first_name + " " + client.client_name;

                var emp = db.Employees.Single(x => x.employee_id == order.order_employee);
                string fullEmpName = emp.employee_first_name + " " + emp.employee_name;

                var productsAssignedToOrder = db.Ord_Prod.Where(x => x.order_id == order.order_id);
                decimal totalPrice = 0;
                foreach (var p in productsAssignedToOrder)
                {
                    var price = db.Products.Single(x => x.product_id == p.product_id).price;
                    totalPrice += price * p.quantity;
                }
                ordersList.Add(new Order { orderId = order.order_id, orderDate = orderDate, clientId = client.client_id, clientFullName = fullClientName, employeeId = emp.employee_id, employeeFullName = fullEmpName, totalPrice = totalPrice });
            }
            foreach (var order in ordersList)
                OrdersDG.Items.Add(order);
        }
        private void FillOrderProducts(object sender, SelectionChangedEventArgs e)
        {
            RefreshProductsInOrder();
        }
        private void RefreshProductsInOrder()
        {
            productsInOrderList.Clear();
            ProductsInOrderDG.Items.Clear();
            if (OrdersDG.SelectedItem == null) return;
            int selectedOrderId = ((Order)OrdersDG.SelectedItem).orderId;
            Orders selectedOrder = db.Orders.Single(x => x.order_id == selectedOrderId);
            foreach (var item in selectedOrder.Ord_Prod)
            {
                var product = db.Products.Single(x => x.product_id == item.product_id);
                decimal sum = product.price * item.quantity;
                string producer = db.Producers.Single(x => x.producer_id == product.producer).producer_name;
                string category = db.Categories.Single(x => x.category_id == product.category).category_name;
                productsInOrderList.Add(new DetailedProductOrder { /*orderId = selectedOrder.order_id, */productId = product.product_id, productName = product.product_name, price = product.price, quantity = item.quantity, sum = sum, productCategory = category, producer = producer });
            }
            foreach (var item in productsInOrderList)
                ProductsInOrderDG.Items.Add(item);
        }
        private void DeleteOrder_Click(object sender, RoutedEventArgs e)
        {
            if (OrdersDG.SelectedItem == null) return;
            int selectedOrderId = ((Order)OrdersDG.SelectedItem).orderId;
            Orders selectedOrder = db.Orders.Single(x => x.order_id == selectedOrderId);
            db.Orders.Remove(selectedOrder);
            db.SaveChanges();
            RefreshOrders();
            MessageBox.Show($"Order successfully deleted.");
        }

        private void CreateOrder_Click(object sender, RoutedEventArgs e)
        {
            if(createOrderPage == null)
                createOrderPage = new CreateOrderPage(db, this.commander);
            this.NavigationService.Navigate(createOrderPage);

        }
    }
}
