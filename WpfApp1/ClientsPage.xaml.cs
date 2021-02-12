using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
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

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for ClientsPage.xaml
    /// </summary>
    public partial class ClientsPage : Page
    {
        MydbEntities db;
        OSHome commander;
        CollectionViewSource clientsViewSource;
        List<ItemsForClientOrderDG> clientOrdersList = new List<ItemsForClientOrderDG>();
        public ClientsPage(MydbEntities db, OSHome commander)
        {
            InitializeComponent();
            this.db = db;
            this.commander = commander;
            clientsViewSource = ((CollectionViewSource)(FindResource("clientsViewSource")));
            DataContext = this;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            db.Clients.Load();
            clientsViewSource.Source = db.Clients.Local;
        }
        
        private void IdSelection(object sender, SelectionChangedEventArgs e)
        {
            clientOrdersList.Clear();
            ClientOrdersDG.Items.Clear();
            var s = string.Join(", ", clientsDataGrid.SelectedCells
                        .Select(cl => cl.Item.GetType()
                                             .GetProperty(cl.Column.SortMemberPath)
                                             .GetValue(cl.Item, null)));
            int clientId = int.Parse(s.Split(',')[0]);
            var clientOrders = db.Orders.Where(x => x.client_id == clientId);
            foreach(var order in clientOrders)
            {
                string orderDate = order.order_date.ToString("MM/dd/yyyy hh:mm tt");
                var emp = db.Employees.Single(x => x.employee_id == order.order_employee);
                string fullEmpName = emp.employee_first_name + " " + emp.employee_name;
                var productsAssignedToOrder = db.Ord_Prod.Where(x => x.order_id == order.order_id);
                decimal totalPrice = 0;
                foreach(var p in productsAssignedToOrder)
                {
                    var price = db.Products.Single(x => x.product_id == p.product_id).price;
                    totalPrice += price * p.quantity;
                }
                clientOrdersList.Add(new ItemsForClientOrderDG {clientId = clientId, orderId = order.order_id, orderDate = orderDate, employeeFullName = fullEmpName, totalPrice = totalPrice });
            }
            foreach (var item in clientOrdersList)
                ClientOrdersDG.Items.Add(item);
        }
        class ItemsForClientOrderDG
        {
            public int clientId { get; set; }
            public int orderId { get; set; }
            public string orderDate { get; set; }
            public string employeeFullName { get; set; }
            public decimal totalPrice { get; set; }
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Find_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Update_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
