using System;
using System.Collections.Generic;
using System.Text;

namespace ClassLibrary1
{
    public class ProductOrder
    {
        public int orderId { get; set; }
        public int productId { get; set; }
        public string productName { get; set; }
        public decimal price { get; set; }
        public int quantity { get; set; }
        public decimal sum { get; set; }
    }
}
