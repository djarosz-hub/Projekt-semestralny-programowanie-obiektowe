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
        List<ClientOrder> clientOrdersList = new List<ClientOrder>();
        List<ProductOrder> productsInOrderList = new List<ProductOrder>();
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
        private void ClientSelection(object sender, SelectionChangedEventArgs e)
        {
            clientOrdersList.Clear();
            ClientOrdersDG.Items.Clear();
            int clientId = ((Clients)clientsDataGrid.SelectedItem).client_id;
            var clientOrders = db.Orders.Where(x => x.client_id == clientId);
            foreach (var order in clientOrders)
            {
                string orderDate = order.order_date.ToString("MM/dd/yyyy hh:mm tt");
                var emp = db.Employees.Single(x => x.employee_id == order.order_employee);
                string fullEmpName = emp.employee_first_name + " " + emp.employee_name;
                var productsAssignedToOrder = db.Ord_Prod.Where(x => x.order_id == order.order_id);
                decimal totalPrice = 0;
                foreach (var p in productsAssignedToOrder)
                {
                    var price = db.Products.Single(x => x.product_id == p.product_id).price;
                    totalPrice += price * p.quantity;
                }
                clientOrdersList.Add(new ClientOrder { clientId = clientId, orderId = order.order_id, orderDate = orderDate, employeeFullName = fullEmpName, totalPrice = totalPrice });
            }
            foreach (var item in clientOrdersList)
                ClientOrdersDG.Items.Add(item);
        }
        private void ClientOrderSelection(object sender, SelectionChangedEventArgs e)
        {
            productsInOrderList.Clear();
            ProductsInOrderDG.Items.Clear();
            if (ClientOrdersDG.SelectedItem == null) return;
            int orderId = ((ClientOrder)ClientOrdersDG.SelectedItem).orderId;
            var productsInOrder = db.Ord_Prod.Where(x => x.order_id == orderId);
            foreach(var p in productsInOrder)
            {
                var product = db.Products.Single(x => x.product_id == p.product_id);
                decimal sum = p.quantity * product.price;
                productsInOrderList.Add(new ProductOrder { orderId = orderId, productId = p.product_id, productName = product.product_name, price = product.price, quantity = p.quantity, sum = sum });
            }
            foreach (var item in productsInOrderList)
                ProductsInOrderDG.Items.Add(item);
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            string firstName = AddFirstNameTB.Text.Trim();
            string lastName = AddLastNameTB.Text.Trim();
            string fen = AddFENTB.Text.Trim();
            if (commander.IsInputInvalid(firstName, "First name") || commander.IsInputInvalid(lastName, "Last name"))
            {
                MessageBox.Show($"Invalid name value.");
                return;
            }
            if (!commander.LettersInputOnly(firstName))
            {
                MessageBox.Show($"First name must consist of letter only.");
                return;
            }
            if (!commander.LettersInputOnly(lastName))
            {
                MessageBox.Show($"Last name must consist of letter only.");
                return;

            }
            if(fen.Length > 0 && fen.Length < 8)
            {
                MessageBox.Show($"FEN must consist of 8-10 letters or digits or must be empty.");
                return;
            }
            if (!commander.LettersAndDigitsInputOnly(fen))
            {
                MessageBox.Show($"FEN must consist of letters or digits only, {fen} is not valid FEN number");
                return;
            }
            var duplicatedClient = db.Clients.SingleOrDefault(x =>
                 x.client_first_name.ToLower() == firstName.ToLower() &&
                 x.client_name.ToLower() == lastName.ToLower() &&
                 x.firm_evidence_number.ToLower() == fen.ToLower());
            if (duplicatedClient != null)
            {
                MessageBox.Show($"Client already exists.\nID: {duplicatedClient.client_id}, {duplicatedClient.client_first_name} {duplicatedClient.client_name}, FEN: {duplicatedClient.firm_evidence_number}");
                return;
            }
            Clients newClient = new Clients();
            newClient.client_first_name = firstName;
            newClient.client_name = lastName;
            newClient.firm_evidence_number = fen;
            db.Clients.Add(newClient);
            db.SaveChanges();
            MessageBox.Show($"Successfully created client:\nID: {newClient.client_id}, {newClient.client_first_name} {newClient.client_name}, FEN: {newClient.firm_evidence_number}");
            AddFirstNameTB.Text = "First name";
            AddLastNameTB.Text = "Last name";
            AddFENTB.Text = "FEN";
        }
        private void Find_Click(object sender, RoutedEventArgs e)
        {
            string firstName = FindFirstNameTB.Text.Trim();
            string lastName = FindLastNameTB.Text.Trim();
            if (commander.IsInputInvalid(firstName, "First name") || commander.IsInputInvalid(lastName, "Last name"))
            {
                MessageBox.Show($"Invalid or empty searching criteria.");
                return;
            }
            var clientsFullName = db.Clients.Where(x => x.client_first_name.ToLower() == firstName.ToLower() && x.client_name.ToLower() == lastName.ToLower());
            StringBuilder sb = new StringBuilder();
            if (clientsFullName.Count() > 0)
            {
                foreach (var c in clientsFullName)
                {
                    sb.Append($"ID: {c.client_id}, {c.client_first_name} {c.client_name}, FEN: {c.firm_evidence_number}\n");
                }
                MessageBox.Show($"Found:\n{sb}");
                FindFirstNameTB.Text = "First name";
                FindLastNameTB.Text = "Last name";
                return;
            }
            else
            {
                var clientsContainingFirstName = db.Clients.Where(x => x.client_first_name.ToLower() == firstName.ToLower());
                var clientsContainingLastName = db.Clients.Where(x => x.client_name.ToLower() == lastName.ToLower());
                if (clientsContainingFirstName.Count() > 0)
                {
                    sb.Append($"Clients found by first name:\n");
                    foreach (var c in clientsContainingFirstName)
                    {
                        sb.Append($"ID: {c.client_id}, {c.client_first_name} {c.client_name}, FEN: {c.firm_evidence_number}\n");
                    }
                }
                if (clientsContainingLastName.Count() > 0)
                {
                    sb.Append($"Clients found by last name:\n");
                    foreach (var c in clientsContainingLastName)
                    {
                        sb.Append($"ID: {c.client_id}, {c.client_first_name} {c.client_name}, FEN: {c.firm_evidence_number}\n");
                    }
                }
                if (sb.Length > 0)
                {
                    MessageBox.Show($"{sb}");
                    FindFirstNameTB.Text = "First name";
                    FindLastNameTB.Text = "Last name";
                    return;
                }
            }
            MessageBox.Show($"No client match searching criteria.");
            FindFirstNameTB.Text = "First name";
            FindLastNameTB.Text = "Last name";
        }
        private void Update_Click(object sender, RoutedEventArgs e)
        {
            string stringNewFen = UpdateFENTB.Text.Trim();
            string stringId = UpdateIdNTB.Text.Trim();
            if (commander.IsInputInvalid(stringNewFen, "New FEN") || commander.IsInputInvalid(stringId, "ID"))
            {
                MessageBox.Show($"Invalid input");
                return;
            }
            if (stringNewFen.Length < 8)
            {
                MessageBox.Show($"FEN must consist of 8-10 letter or digits.");
                return;
            }
            if (!commander.LettersAndDigitsInputOnly(stringNewFen))
            {
                MessageBox.Show($"FEN must consist of letters or digits only, {stringNewFen} is not valid FEN number");
                return;
            }
            if (!int.TryParse(stringId, out int id))
            {
                MessageBox.Show($"Invalid id.");
                return;
            }
            if (!commander.ExistsInDatabaseByID(OSHome.DbSources.Clients, id))
            {
                MessageBox.Show($"No client with ID: {id} in database.");
                return;
            }
            Clients existingClient = db.Clients.Single(x => x.client_id == id);
            if (existingClient.firm_evidence_number == stringNewFen)
            {
                MessageBox.Show($"New FEN is same as existing FEN.");
                return;
            }
            existingClient.firm_evidence_number = stringNewFen;
            db.SaveChanges();
            clientsDataGrid.Items.Refresh();
            MessageBox.Show($"Successfully updated FEN number for client: ID: {existingClient.client_id}, {existingClient.client_first_name} {existingClient.client_name}.");
            UpdateFENTB.Text = "New FEN";
            UpdateIdNTB.Text = "ID";
        }
        public void RemovePlaceHolder(object sender, EventArgs e)
        {
            string[] basics = new string[] { "ID", "First name", "Last name", "FEN", "New FEN" };
            TextBox tb = sender as TextBox;
            if (basics.Contains(tb.Text))
            {
                tb.Text = "";
            }
        }
        class ClientOrder
        {
            public int clientId { get; set; }
            public int orderId { get; set; }
            public string orderDate { get; set; }
            public string employeeFullName { get; set; }
            public decimal totalPrice { get; set; }
        }
        class ProductOrder
        {
            public int orderId { get; set; }
            public int productId { get; set; }
            public string productName { get; set; }
            public decimal price { get; set; }
            public int quantity { get; set; }
            public decimal sum { get; set; }
        }
        
    }
}
