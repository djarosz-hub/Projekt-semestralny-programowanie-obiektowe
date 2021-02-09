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

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for OSHome.xaml
    /// </summary>
    public partial class OSHome : Page
    {
        public enum DbSources { Orders, Categories, Producers, Products, Employees, Clients };
        OrdersPage ordersPage;
        CategoriesPage categoriesPage;
        ProducersPage producersPage;
        ProductsPage productsPage;
        EmployeesPage employeesPage;
        ClientsPage clientsPage;
        internal MydbEntities db;
        public OSHome()
        {
            InitializeComponent();
            db = new MydbEntities();
            ordersPage = new OrdersPage(db);
            categoriesPage = new CategoriesPage(db);
            producersPage = new ProducersPage(db, this);
            productsPage = new ProductsPage(db);
            employeesPage = new EmployeesPage(db);
            clientsPage = new ClientsPage(db);
        }
        internal bool IsInputInvalid(string input, string pattern) => (string.IsNullOrEmpty(input) || input == pattern) ? true : false;
        internal bool IsAssignedToEntity(DbSources sourceCat, int index)
        {
            switch (sourceCat)
            {
                case DbSources.Producers:
                    {
                        foreach(var p in db.Products)
                        {
                            if (p.category == index)
                                return true;
                        }
                        break;
                    }
                case DbSources.Products:
                    break;
                case DbSources.Employees:
                    break;
                case DbSources.Clients:
                    break;
            }
            return false;
        }
        private void OrderButton_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(ordersPage);
        }
        private void CategoriesButton_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(categoriesPage);
        }
        private void ProducersButton_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(producersPage);
        }
        private void ProductsButton_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(productsPage);
        }
        private void EmployeesButton_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(employeesPage);
        }
        private void ClientsButton_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(clientsPage);
        }
    }
}
