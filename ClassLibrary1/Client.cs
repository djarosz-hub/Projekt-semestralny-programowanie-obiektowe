using System;
using System.Collections.Generic;
using System.Text;

namespace ClassLibrary1
{
    /// <summary>
    /// Describes client with concatenated first and last name. Used to simply show information about client.
    /// </summary>
    public class Client
    {
        public int clientId { get; set; }
        public string clientFullName { get; set; }
        public string firmEvidenceNumber { get; set; }
    }
}
