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
        readonly OrdersPage ordersPage;
        readonly CategoriesPage categoriesPage;
        readonly ProducersPage producersPage;
        readonly ProductsPage productsPage;
        readonly EmployeesPage employeesPage;
        readonly ClientsPage clientsPage;
        public OSHome()
        {
            InitializeComponent();
            ordersPage = new OrdersPage();
            categoriesPage = new CategoriesPage();
            producersPage = new ProducersPage();
            productsPage = new ProductsPage();
            employeesPage = new EmployeesPage();
            clientsPage = new ClientsPage();
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
