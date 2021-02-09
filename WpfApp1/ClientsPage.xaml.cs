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
    /// Interaction logic for ClientsPage.xaml
    /// </summary>
    public partial class ClientsPage : Page
    {
        MydbEntities db;
        OSHome commander;
        CollectionViewSource clientsOrderViewSource;
        CollectionViewSource clientsViewSource;
        public ClientsPage(MydbEntities db, OSHome commander)
        {
            InitializeComponent();
            this.db = db;
            this.commander = commander;
            clientsOrderViewSource = ((CollectionViewSource)(FindResource("clientsOrdersViewSource")));
            clientsViewSource = ((CollectionViewSource)(FindResource("clientsViewSource")));
            DataContext = this;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            db.Clients.Load();
            clientsViewSource.Source = db.Clients.Local;
        }
    }
}
