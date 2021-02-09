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
    /// Interaction logic for OrdersPage.xaml
    /// </summary>
    public partial class OrdersPage : Page
    {
        MydbEntities db;
        public OrdersPage(MydbEntities db)
        {
            InitializeComponent();
            this.db = db;
            FillOrders();
        }
        void FillOrders()
        {
            var dbOrders = db.Orders;
            foreach(var order in dbOrders)
            {
                string output = $"date: {order.order_date}, client id:{order.client_id}, employee: {order.order_employee}\n";
                testbox.Text += output;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            FillOrders();
        }
    }
}
