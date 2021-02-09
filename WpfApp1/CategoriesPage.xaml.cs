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
        public CategoriesPage(MydbEntities db)
        {
            InitializeComponent();
            this.db = db;
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
            
        }

        private void Update_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Remove_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            
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
