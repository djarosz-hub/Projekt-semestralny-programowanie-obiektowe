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
            categoriesPage = new CategoriesPage(db, this);
            producersPage = new ProducersPage(db, this);
            productsPage = new ProductsPage(db);
            employeesPage = new EmployeesPage(db, this);
            clientsPage = new ClientsPage(db, this);
        }
        internal bool LettersInputOnly(string input)
        {
            foreach (char c in input)
                if (!char.IsLetter(c))
                    return false;
            return true;
        }
        internal bool IsInputInvalid(string input, string pattern) => (string.IsNullOrEmpty(input) || input == pattern) ? true : false;
        internal bool IsAssignedToEntity(DbSources sourceCat, int index)
        {
            switch (sourceCat)
            {
                case DbSources.Producers:
                    {
                        foreach (var p in db.Products)
                        {
                            if (p.producer == index)
                                return true;
                        }
                        break;
                    }
                case DbSources.Categories:
                    {
                        foreach (var c in db.Products)
                        {
                            if (c.category == index)
                                return true;
                        }
                        break;
                    }
                case DbSources.Products:
                    break;
                case DbSources.Employees:
                    {
                        foreach (var o in db.Orders)
                        {
                            if (o.order_employee == index)
                                return true;
                        }
                        break;
                    }
                case DbSources.Clients:
                    break;
            }
            return false;
        }
        internal bool ExistsInDatabaseByID(DbSources sourceCat, int index)
        {
            switch (sourceCat)
            {
                case DbSources.Producers:
                    {
                        foreach (var p in db.Producers)
                        {
                            if (p.producer_id == index)
                                return true;
                        }
                        break;
                    }
                case DbSources.Categories:
                    {
                        foreach (var c in db.Categories)
                        {
                            if (c.category_id == index)
                                return true;
                        }
                        break;
                    }
                case DbSources.Products:
                    break;
                case DbSources.Employees:
                    {
                        foreach (var e in db.Employees)
                        {
                            if (e.employee_id == index)
                                return true;
                        }
                    }
                    break;
                case DbSources.Clients:
                    break;
            }
            return false;
        }
        internal bool ExistsInDatabaseByNameCaseInsensitive(DbSources sourceCat, string input, out string found)
        {
            switch (sourceCat)
            {
                case DbSources.Producers:
                    {
                        foreach (var p in db.Producers)
                        {
                            if (p.producer_name.ToLower() == input.ToLower())
                            {
                                found = p.producer_name;
                                return true;
                            }
                        }
                        break;
                    }
                case DbSources.Categories:
                    {
                        foreach (var c in db.Categories)
                        {
                            if (c.category_name.ToLower() == input.ToLower())
                            {
                                found = c.category_name;
                                return true;
                            }
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
            found = "";
            return false;
        }
        internal bool ExistsInDatabaseByNameCaseSensitive(DbSources sourceCat, string input)
        {
            switch (sourceCat)
            {
                case DbSources.Producers:
                    {
                        foreach (var p in db.Producers)
                        {
                            if (p.producer_name == input)
                                return true;
                        }
                        break;
                    }
                case DbSources.Categories:
                    {
                        foreach (var c in db.Categories)
                        {
                            if (c.category_name == input)
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
        internal void RemoveFromDb(DbSources sourceCat, int index)
        {
            switch (sourceCat)
            {
                case DbSources.Producers:
                    {
                        Producers producerToRemove = db.Producers.Single(x => x.producer_id == index);
                        db.Producers.Remove(producerToRemove);
                        db.SaveChanges();
                        break;
                    }
                case DbSources.Categories:
                    {
                        Categories categoryToRemove = db.Categories.Single(x => x.category_id == index);
                        db.Categories.Remove(categoryToRemove);
                        db.SaveChanges();
                        break;
                    }
                case DbSources.Products:
                    break;
                case DbSources.Employees:
                    {
                        Employees employeeToRemove = db.Employees.Single(x => x.employee_id == index);
                        db.Employees.Remove(employeeToRemove);
                        db.SaveChanges();
                        break;
                    }
                case DbSources.Clients:
                    break;
            }
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
