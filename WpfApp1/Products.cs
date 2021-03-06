//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WpfApp1
{
    using System;
    using System.Collections.ObjectModel;
    
    /// <summary>
    /// Describes sql table modeling product, includes info about orders to which product is assigned by ord_prod class.
    /// </summary>
    public partial class Products
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Products()
        {
            this.Ord_Prod = new ObservableCollection<Ord_Prod>();
        }
    
        public int product_id { get; set; }
        public string product_name { get; set; }
        public short category { get; set; }
        public short producer { get; set; }
        public decimal price { get; set; }
    
        public virtual Categories Categories { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ObservableCollection<Ord_Prod> Ord_Prod { get; set; }
        public virtual Producers Producers { get; set; }
    }
}
