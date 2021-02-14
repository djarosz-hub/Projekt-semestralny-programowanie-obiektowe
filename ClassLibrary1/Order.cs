using System;
using System.Collections.Generic;
using System.Text;

namespace ClassLibrary1
{
    public class Order
    {
        public int orderId { get; set; }
        public string orderDate { get; set; }
        public int clientId { get; set; }
        public string clientFullName { get; set; }
        public int employeeId { get; set; }
        public string employeeFullName { get; set; }
        public decimal totalPrice { get; set; }
    }
}
