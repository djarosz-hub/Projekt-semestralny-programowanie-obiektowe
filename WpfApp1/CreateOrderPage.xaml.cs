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
    /// </summary>
    public partial class CreateOrderPage : Page
    {
        MydbEntities db;
        OSHome commander;
        string mostRecentClientName = "";
        string mostRecentEmployeeName = "";
        //string mostRecentProductText = "";
        List<Clients> clientsList = new List<Clients>();
        List<Employees> employeesList = new List<Employees>();
        public CreateOrderPage(MydbEntities db, OSHome commander)
        {
            InitializeComponent();
            this.db = db;
            this.commander = commander;
            FillAllClients();
            FillAllEmployees();
        }

        private void CreateNewOrder_Click(object sender, RoutedEventArgs e)
        {

        }
        void FillAllClients()
        {
            clientsList.Clear();
            ClientsDG.Items.Clear();
            foreach (var c in db.Clients)
                ClientsDG.Items.Add(c);
        }
        async void Client_textChanged(object sender, TextChangedEventArgs e)
        {
            string enteredText = ClientNameTB.Text;
            mostRecentClientName = enteredText;
            await Task.Delay(500);
            clientsList.Clear();
            ClientsDG.Items.Clear();
            if(ClientNameTB.Text == "")
            {
                FillAllClients();
                return;
            }
            if (enteredText == mostRecentClientName)
            {
                var selectedClients = db.Clients.Where(x => x.client_name.Contains(mostRecentClientName));
                foreach (var c in selectedClients)
                    clientsList.Add(c);
                foreach (var c in clientsList)
                    ClientsDG.Items.Add(c);
            }
        }
        void FillAllEmployees()
        {
            employeesList.Clear();
            EmployeesDG.Items.Clear();
            foreach (var emp in db.Employees)
                EmployeesDG.Items.Add(emp);
        }
        async void Employee_textChanged(object sender, TextChangedEventArgs e)
        {
            string enteredText = EmployeeNameTB.Text;
            mostRecentEmployeeName = enteredText;
            await Task.Delay(500);
            employeesList.Clear();
            EmployeesDG.Items.Clear();
            if(EmployeeNameTB.Text == "")
            {
                FillAllEmployees();
                return;
            }
            if(enteredText == mostRecentEmployeeName)
            {
                var selectedEmployees = db.Employees.Where(x => x.employee_name.Contains(mostRecentEmployeeName));
                foreach (var emp in selectedEmployees)
                    employeesList.Add(emp);
                foreach (var emp in employeesList)
                    EmployeesDG.Items.Add(emp);
            }
        }

        private void Product_textChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
