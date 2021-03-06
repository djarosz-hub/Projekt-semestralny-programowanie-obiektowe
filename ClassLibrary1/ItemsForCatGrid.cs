﻿using System;
using System.Collections.Generic;
using System.Text;

namespace ClassLibrary1
{
    /// <summary>
    /// Clearly describes item when it's necessary to simply identify product when filtering.
    /// </summary>
    public class ItemsForCatGrid
    {
        public int id { get; set; }
        public string product_name { get; set; }
        public string producer { get; set; }
        public decimal price { get; set; }
    }
}
