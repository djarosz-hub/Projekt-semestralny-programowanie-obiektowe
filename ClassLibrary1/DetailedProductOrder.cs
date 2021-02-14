using System;
using System.Collections.Generic;
using System.Text;

namespace ClassLibrary1
{
    public class DetailedProductOrder : ProductOrder
    {
        public string productCategory { get; set; }
        public string producer { get; set; }
    }
}
