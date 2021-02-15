using System;
using System.Collections.Generic;
using System.Text;

namespace ClassLibrary1
{
    /// <summary>
    /// Extends ProductOrder class by additional category and producer property, gives detailed info about order.
    /// </summary>
    public class DetailedProductOrder : ProductOrder
    {
        public string productCategory { get; set; }
        public string producer { get; set; }
    }
}
