using System;
using System.Collections.Generic;
using System.Text;

namespace ClassLibrary1
{
    public class ClientOrder
    {
        public int clientId { get; set; }
        public int orderId { get; set; }
        public string orderDate { get; set; }
        public string employeeFullName { get; set; }
        public decimal totalPrice { get; set; }
    }
}
