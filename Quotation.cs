using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterportCargoWPF
{
    public class Quotation
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public string Source { get; set; }
        public string Destination { get; set; }
        public int NumberOfContainers { get; set; }
        public string NatureOfJob { get; set; }

        // Navigation property to link the quotation with a customer
        public Customer Customer { get; set; }
    }

}
