using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace InterportCargoWPF.Models;

public class Quotation
{
    public int Id { get; set; }

    public int CustomerId { get; set; }
    public string Origin { get; set; } = string.Empty;
    public string Destination { get; set; } = string.Empty;
    public string CargoType { get; set; } = string.Empty;
    public int ContainerQuantity { get; set; }
    public string NatureOfJob { get; set; } = string.Empty;
    public DateTime TransportationDate { get; set; }
    public string Status { get; set; } = "Pending";

    public decimal TotalAmount { get; set; } // Initial amount before discounts
    public decimal Discount { get; set; } // Discount rate (e.g., 0.05 for 5%)
    public decimal FinalAmount { get; set; } // Final amount after discounts

    public string? AdditionalRequirements { get; set; }

    [ForeignKey("CustomerId")]
    public Customer? Customer { get; set; } // Nullable to handle cases where it might be missing

    /// <summary>
    /// Calculates the initial amount based on container type and quantity.
    /// </summary>
    public void CalculateInitialAmount()
    {
        // Define base rates for container types
        var ratePerContainer = CargoType == "20 Feet" ? 60.0m : 70.0m; // Example rates
        TotalAmount = ratePerContainer * ContainerQuantity;

        // Set the FinalAmount initially to the TotalAmount
        FinalAmount = TotalAmount;
    }

    /// <summary>
    /// Applies a discount based on business rules and updates the FinalAmount.
    /// </summary>
    public void ApplyDiscount()
    {
        // Example discount logic based on container quantity and job type
        if (ContainerQuantity > 10 && (NatureOfJob.Contains("Quarantine") || NatureOfJob.Contains("Fumigation")))
            Discount = 0.10m; // 10% discount
        else if (ContainerQuantity > 5 && (NatureOfJob.Contains("Quarantine") || NatureOfJob.Contains("Fumigation")))
            Discount = 0.05m; // 5% discount
        else if (ContainerQuantity > 5 && NatureOfJob.Contains("Quarantine") && NatureOfJob.Contains("Fumigation"))
            Discount = 0.025m; // 2.5% discount
        else
            Discount = 0m; // No discount

        // Calculate the FinalAmount after applying the discount
        FinalAmount = TotalAmount * (1 - Discount);
    }
}