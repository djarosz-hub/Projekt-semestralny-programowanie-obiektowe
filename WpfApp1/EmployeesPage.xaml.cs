using System;
using System.Collections.Generic;
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
    /// Interaction logic for EmployeesPage.xaml
    /// Page enables to handle employees table and show to which orders employee is assigned.
    /// </summary>
    public partial class EmployeesPage : Page
    {
        MydbEntities db;
        OSHome commander;
        CollectionViewSource empViewSource;
        CollectionViewSource empOrdersViewSource;
        public EmployeesPage(MydbEntities db, OSHome commander)
        {
            InitializeComponent();
            this.db = db;
            this.commander = commander;
            empViewSource = ((CollectionViewSource)(FindResource("employeesViewSource")));
            empOrdersViewSource = ((CollectionViewSource)(FindResource("employeesOrdersViewSource")));
            DataContext = this;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            db.Employees.Load();
            empViewSource.Source = db.Employees.Local;
        }
        private void Find_Click(object sender, RoutedEventArgs e)
        {
            string firstName = FindFirstNameTB.Text.Trim();
            string lastName = FindLastNameTB.Text.Trim();
            if(commander.IsInputInvalid(firstName,"First name") || commander.IsInputInvalid(lastName,"Last name") || !commander.LettersInputOnly(firstName) || !commander.LettersInputOnly(lastName))
            {
                MessageBox.Show("Invalid first name or last name values.");
                return;
            }
            var foundEmployees = db.Employees.Where(x => x.employee_first_name.ToLower() == firstName.ToLower() && x.employee_name.ToLower() == lastName.ToLower());
            if(foundEmployees.Count() == 0)
            {
                MessageBox.Show($"No employees named {firstName} {lastName} found.");
                return;
            }
            StringBuilder sb = new StringBuilder();
            foreach(var emp in foundEmployees)
            {
                sb.Append($"ID: {emp.employee_id}, {emp.employee_first_name} {emp.employee_name}\n");
            }
            MessageBox.Show($"Employees found:\n{sb}");
            FindFirstNameTB.Text = "First name";
            FindLastNameTB.Text = "Last name";
        }
        private void Update_Click(object sender, RoutedEventArgs e)
        {
            string stringId = UpdateIDTB.Text.Trim();
            string newLastName = UpdateLastNameTB.Text.Trim();
            if (commander.IsInputInvalid(stringId, "ID") || !int.TryParse(stringId, out int id) || !commander.LettersInputOnly(newLastName))
            {
                MessageBox.Show($"{stringId} is not valid id.");
                return;
            }
            if (!commander.ExistsInDatabaseByID(OSHome.DbSources.Employees, id))
            {
                MessageBox.Show($"Employee doesn't exist in database.");
                return;
            }
            var emp = db.Employees.Single(x => x.employee_id == id);
            string oldLastName = emp.employee_name;
            if(oldLastName == newLastName)
            {
                MessageBox.Show($"New last name is same as existing last name.");
                return;
            }
            if (!commander.FinalAcceptancePrompt())
                return;
            emp.employee_name = newLastName;
            db.SaveChanges();
            employeesDataGrid.Items.Refresh();
            UpdateIDTB.Text = "ID";
            UpdateLastNameTB.Text = "New last name";
            MessageBox.Show($"Successfully updated employee: {emp.employee_first_name} {oldLastName} into: {emp.employee_first_name} {newLastName}");
        }
        private void Remove_Click(object sender, RoutedEventArgs e)
        {
            string stringId = RemoveIDTB.Text.Trim();
            if (commander.IsInputInvalid(stringId, "ID") || !int.TryParse(stringId, out int id))
            {
                MessageBox.Show($"{stringId} is not valid id.");
                return;
            }
            if(!commander.ExistsInDatabaseByID(OSHome.DbSources.Employees,id) || commander.IsAssignedToEntity(OSHome.DbSources.Employees, id))
            {
                MessageBox.Show($"Employee doesn't exist in database or is already assigned to some order");
                return;
            }
            if (!commander.FinalAcceptancePrompt())
                return;
            commander.RemoveFromDb(OSHome.DbSources.Employees, id);
            MessageBox.Show("Employee successfully removed from database.");
            RemoveIDTB.Text = "ID";
        }
        private void Add_Click(object sender, RoutedEventArgs e)
        {
            string firstName = AddFirstNameTB.Text.Trim();
            string lastName = AddLastNameTB.Text.Trim();
            if (commander.IsInputInvalid(firstName, "First name") || commander.IsInputInvalid(lastName, "Last name") || !commander.LettersInputOnly(firstName) || !commander.LettersInputOnly(lastName))
            {
                MessageBox.Show("Invalid first name or last name values.");
                return;
            }
            Employees newEmp = new Employees();
            newEmp.employee_first_name = firstName;
            newEmp.employee_name = lastName;
            db.Employees.Add(newEmp);
            db.SaveChanges();
            AddFirstNameTB.Text = "First name";
            AddLastNameTB.Text = "Last name";
            MessageBox.Show($"Successfully added new employee: {firstName} {lastName}");
        }
        public void RemovePlaceHolder(object sender, EventArgs e)
        {
            string[] basics = new string[] { "ID", "First name", "Last name", "New last name" };
            TextBox tb = sender as TextBox;
            if (basics.Contains(tb.Text))
            {
                tb.Text = "";
            }
        }
    }
}
