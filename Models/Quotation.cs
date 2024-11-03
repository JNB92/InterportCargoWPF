using System.ComponentModel.DataAnnotations.Schema;

namespace InterportCargoWPF.Models
{
    public class Quotation
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public string Origin { get; set; }
        public string Destination { get; set; }
        public string CargoType { get; set; }
        public int ContainerQuantity { get; set; }
        public string NatureOfJob { get; set; }

        // Navigation property to link the quotation with a customer
        [ForeignKey("CustomerId")] public Customer Customer { get; set; }
    }
}