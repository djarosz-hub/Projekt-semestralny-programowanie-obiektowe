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
    /// Interaction logic for CategoriesPage.xaml
    /// </summary>
    public partial class CategoriesPage : Page
    {
        MydbEntities db;
        CollectionViewSource categoriesViewSource;
        OSHome commander;
        public CategoriesPage(MydbEntities db, OSHome commander)
        {
            InitializeComponent();
            this.db = db;
            this.commander = commander;
            categoriesViewSource = ((CollectionViewSource)(FindResource("categoriesViewSource")));
            DataContext = this;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            db.Categories.Load();
            categoriesViewSource.Source = db.Categories.Local;
        }
        private void Find_Click(object sender, RoutedEventArgs e)
        {
            string value = FindTB.Text.Trim();
            if(commander.IsInputInvalid(value, "Category name"))
            {
                MessageBox.Show("Invalid value");
                return;
            }
            if(commander.ExistsInDatabaseByNameCaseInsensitive(OSHome.DbSources.Categories, value, out string found))
            {
                int categoryId = db.Categories.Single(x => x.category_name == found).category_id;
                MessageBox.Show($"Found:\nID: {categoryId}\nName: {found}");
                return;
            }
            MessageBox.Show("Not Found");
            FindTB.Text = "Category name";
        }
        private void Update_Click(object sender, RoutedEventArgs e)
        {
            string newName = UpdateNewNameTB.Text.Trim();
            string oldName = UpdateOldNameTB.Text.Trim();
            if(commander.IsInputInvalid(newName, "New name") || commander.IsInputInvalid(oldName,"Old name"))
            {
                MessageBox.Show("Empty or default value is invalid for updating");
                return;
            }
            if(commander.ExistsInDatabaseByNameCaseInsensitive(OSHome.DbSources.Categories, newName, out string found))
            {
                MessageBox.Show($"Category named {newName} already exists in database.");
                return;
            }
            if(!commander.ExistsInDatabaseByNameCaseSensitive(OSHome.DbSources.Categories, oldName))
            {
                MessageBox.Show($"Category named {oldName} doesn't exists in database.");
                return;
            }

            Categories categoryToUpdate = new Categories();
            categoryToUpdate = db.Categories.Single(x => x.category_name == oldName);
            categoryToUpdate.category_name = newName;
            db.SaveChanges();
            categoriesDataGrid.Items.Refresh();
            UpdateNewNameTB.Text = "New name";
            UpdateOldNameTB.Text = "Old name";
            MessageBox.Show($"Successfully updated: {oldName} into: {newName}.");
        }
        private void Remove_Click(object sender, RoutedEventArgs e)
        {
            string value = RemoveNameTB.Text.Trim();
            if(commander.IsInputInvalid(value, "Category name or ID"))
            {
                MessageBox.Show($"Input value is default or empty, type correct value");
                return;
            }
            if(int.TryParse(value, out int parsedIntValue))
            {
                if(!commander.ExistsInDatabaseByID(OSHome.DbSources.Categories, parsedIntValue))
                {
                    MessageBox.Show($"Category with ID: {parsedIntValue} doesn't exists in database.");
                    return;
                }
            }
            else
            {
                if(!commander.ExistsInDatabaseByNameCaseSensitive(OSHome.DbSources.Categories, value))
                {
                    MessageBox.Show($"Producer with name: {value} doesn't exists in database.");
                    return;
                }
            }

            if(parsedIntValue == 0)
            {
                int categoryId = db.Categories.Single(x => x.category_name == value).category_id;
                RemoveCat(categoryId);
            }
            else
            {
                RemoveCat(parsedIntValue);
            }
            MessageBox.Show("Category successfully removed from database.");
            RemoveNameTB.Text = "Category name or ID";
        }
        private void RemoveCat(int index)
        {
            if(commander.IsAssignedToEntity(OSHome.DbSources.Categories, index))
            {
                MessageBox.Show($"Category is already assigned to product, can't remove.");
                return;
            }
            commander.RemoveFromDb(OSHome.DbSources.Categories, index);
        }
        private void Add_Click(object sender, RoutedEventArgs e)
        {
            string value = AddNameTB.Text.Trim();
            if(commander.IsInputInvalid(value, "Category name"))
            {
                MessageBox.Show($"Can't add empty or default value to database.");
                return;
            }
            if(commander.ExistsInDatabaseByNameCaseInsensitive(OSHome.DbSources.Categories, value, out string found))
            {
                MessageBox.Show($"Category with name {found} already exists in database.");
                return;
            }
            Categories newCategory = new Categories();
            newCategory.category_name = value;
            db.Categories.Add(newCategory);
            db.SaveChanges();
            MessageBox.Show($"Successfully added new category to database: {value}");
            AddNameTB.Text = "Category name";
        }
        public void RemovePlaceHolder(object sender, EventArgs e)
        {
            string[] basics = new string[] { "ID", "New name", "Old name", "Category name or ID", "Category name" };
            TextBox tb = sender as TextBox;
            if (basics.Contains(tb.Text))
            {
                tb.Text = "";
            }
        }
    }
}
