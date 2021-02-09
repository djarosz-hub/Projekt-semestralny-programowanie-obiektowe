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
    /// Interaction logic for ProducersPage.xaml
    /// </summary>
    public partial class ProducersPage : Page
    {
        MydbEntities db;
        CollectionViewSource producersViewSource;
        OSHome commander;
        public ProducersPage(MydbEntities db, OSHome commander)
        {
            InitializeComponent();
            this.db = db;
            this.commander = commander;
            producersViewSource = ((CollectionViewSource)(FindResource("producersViewSource")));
            DataContext = this;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            db.Producers.Load();
            producersViewSource.Source = db.Producers.Local;
        }
        public void RemovePlaceHolder(object sender, EventArgs e)
        {
            string[] basics = new string[] { "ID", "New name", "Old name", "Producer name or ID", "Producer name" };
            TextBox tb = sender as TextBox;
            if (basics.Contains(tb.Text))
            {
                tb.Text = "";
            }
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            string value = AddNameTB.Text.Trim();
            if (commander.IsInputInvalid(value, "Producer name"))
            {
                MessageBox.Show($"Can't add empty or default value to database.");
                return;
            }
            if (ExistsInDatabaseByNameCaseInsensitive(value, out string found))
            {
                MessageBox.Show($"Producer with name {found} already exists in database.");
                return;
            }
            Producers newProducer = new Producers();
            newProducer.producer_name = value;
            db.Producers.Add(newProducer);
            db.SaveChanges();
            MessageBox.Show($"Successfully added new producer to database: {value}.");
            AddNameTB.Text = "Producer name";
        }
        private void Remove_Click(object sender, RoutedEventArgs e)
        {
            string value = RemoveNameTB.Text.Trim();
            if (commander.IsInputInvalid(value, "Producer name or ID"))
            {
                MessageBox.Show($"Input value is default or empty, type correct value.");
                return;
            }
            if (int.TryParse(value, out int parsedIntValue))
            {
                if (!ExistsInDatabaseByID(parsedIntValue))
                {
                    MessageBox.Show($"Producer with ID: {parsedIntValue} doesn't exists in database.");
                    return;
                }
            }
            else
            {
                if (!ExistsInDatabaseByNameCaseSensitive(value))
                {
                    MessageBox.Show($"Producer with name: {value} doesn't exists in database.");
                    return;
                }
            }
            if (parsedIntValue == 0)
            {
                int producerId = db.Producers.Single(x => x.producer_name == value).producer_id;
                if(commander.IsAssignedToEntity(OSHome.DbSources.Producers, producerId))
                {
                    MessageBox.Show($"Producer is already assigned to product, can't remove.");
                    return;
                }
                RemoveFromDb(producerId);
            }
            else
            {
                if (commander.IsAssignedToEntity(OSHome.DbSources.Producers, parsedIntValue))
                {
                    MessageBox.Show($"Producer is already assigned to product, can't remove.");
                    return;
                }
                RemoveFromDb(parsedIntValue);
            }
            MessageBox.Show("Producer successfully removed from database.");
            RemoveNameTB.Text = "Producer name or ID";

        }
        private void Update_Click(object sender, RoutedEventArgs e)
        {
            string newName = UpdateNewNameTB.Text.Trim();
            string oldName = UpdateOldNameTB.Text.Trim();
            if(commander.IsInputInvalid(newName, "New name") || commander.IsInputInvalid(oldName, "Old name"))
            {
                MessageBox.Show("Empty or default value is invalid for updating.");
                return;
            }
            if(ExistsInDatabaseByNameCaseInsensitive(newName, out string found))
            {
                MessageBox.Show($"Producer named {newName} already exists in database.");
                return;
            }
            if (!ExistsInDatabaseByNameCaseSensitive(oldName))
            {
                MessageBox.Show($"Producer named {oldName} doesn't exists in database.");
                return;
            }
            Producers producerToUpdate = new Producers();
            producerToUpdate = db.Producers.Single(x => x.producer_name == oldName);
            producerToUpdate.producer_name = newName;
            db.SaveChanges();
            producersDataGrid.Items.Refresh();
            UpdateNewNameTB.Text = "New name";
            UpdateOldNameTB.Text = "Old name";
            MessageBox.Show($"Successfully updated: {oldName} into: {newName}.");
        }
        private void Find_Click(object sender, RoutedEventArgs e)
        {
            string value = FindTB.Text.Trim();
            if (commander.IsInputInvalid(value, "Producer name"))
            {
                MessageBox.Show($"Invalid value");
                return;
            }
            if(ExistsInDatabaseByNameCaseInsensitive(value, out string found))
            {
                int producerId = db.Producers.Single(x => x.producer_name == found).producer_id;
                MessageBox.Show($"Found:\nID: {producerId}\nName: {found}");
                return;
            }
            MessageBox.Show("Not found.");
            FindTB.Text = "Producer name";
        }
        private void RemoveFromDb(int index)
        {
            Producers producerToRemove = db.Producers.Single(x => x.producer_id == index);
            db.Producers.Remove(producerToRemove);
            db.SaveChanges();
        }
        //private bool IsProducerAssignedToAnyProduct(int index)
        //{
        //    foreach (var p in db.Products)
        //    {
        //        if (p.category == index)
        //            return true;
        //    }
        //    return false;
        //}
        //private bool IsInplutInvalid(string input, string pattern) => (string.IsNullOrEmpty(input) || input == pattern) ? true : false;
        private bool ExistsInDatabaseByNameCaseInsensitive(string input, out string found)
        {
            var exisitingProducers = db.Producers;
            foreach (var p in exisitingProducers)
                if (p.producer_name.ToLower() == input.ToLower())
                {
                    found = p.producer_name;
                    return true;
                }
            found = "";
            return false;
        }
        private bool ExistsInDatabaseByNameCaseSensitive(string input)
        {
            var exisitingProducers = db.Producers;
            foreach (var p in exisitingProducers)
                if (p.producer_name == input)
                    return true;
            return false;
        }
        private bool ExistsInDatabaseByID(int input)
        {
            var exisitingProducers = db.Producers;
            foreach (var p in exisitingProducers)
                if (p.producer_id == input)
                    return true;
            return false;
        }

    }
}
