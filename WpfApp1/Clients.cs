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
    /// Defines basic model of sql clients table, includes orders assigned to client
    /// </summary>
    public partial class Clients
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Clients()
        {
            this.Orders = new ObservableCollection<Orders>();
        }
    
        public int client_id { get; set; }
        public string client_name { get; set; }
        public string client_first_name { get; set; }
        public string firm_evidence_number { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ObservableCollection<Orders> Orders { get; set; }
    }
}
