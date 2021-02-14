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
        //string mostRecentEmployeeText = "";
        //string mostRecentProductText = "";
        List<Client> clientsList = new List<Client>();
        public CreateOrderPage(MydbEntities db, OSHome commander)
        {
            InitializeComponent();
            this.db = db;
            this.commander = commander;
            FillAllClients();
        }

        private void CreateNewOrder_Click(object sender, RoutedEventArgs e)
        {

        }
        void FillAllClients()
        {
            clientsList.Clear();
            ClientsDG.Items.Clear();
            foreach (var c in db.Clients)
            {
                clientsList.Add(new Client {clientId = c.client_id, clientFullName = c.client_first_name + " " + c.client_name, firmEvidenceNumber = c.firm_evidence_number });
            }
            foreach (var c in clientsList)
                ClientsDG.Items.Add(c);
        }
        async void client_textChanged(object sender, TextChangedEventArgs e)
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
                {
                    clientsList.Add(new Client {clientId = c.client_id, clientFullName = c.client_first_name + " " + c.client_name, firmEvidenceNumber = c.firm_evidence_number });
                }
                foreach (var c in clientsList)
                    ClientsDG.Items.Add(c);
            }
        }
    }
}
