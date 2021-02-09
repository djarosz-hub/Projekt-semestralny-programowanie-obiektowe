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
using System.Data.Entity;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for ProductsPage.xaml
    /// </summary>
    public partial class ProductsPage : Page
    {
        MydbEntities db;
        CollectionViewSource productViewSource;
        public ProductsPage(MydbEntities db)
        {
            InitializeComponent();
            this.db = db;
            productViewSource = ((CollectionViewSource)(FindResource("productsViewSource")));
            DataContext = this;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            db.Products.Load();
            productViewSource.Source = db.Products.Local;
        }
    }
}
