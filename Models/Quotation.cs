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
        public DateTime TransportationDate { get; set; }
        public string Status { get; set; }

        public decimal TotalAmount { get; set; } // The initial total amount before discounts
        public decimal Discount { get; set; } // Discount rate, e.g., 0.05 for 5%
        public decimal FinalAmount { get; set; } // The final amount after applying discounts
        
        public string? AdditionalRequirements { get; set; }

        // Navigation property to link the quotation with a customer
        [ForeignKey("CustomerId")] public Customer Customer { get; set; }
    }
}