using System.ComponentModel.DataAnnotations.Schema;

namespace InterportCargoWPF.Models;

/// <summary>
///     Represents a quotation for a customer, detailing transportation job specifications, pricing, and discounts.
/// </summary>
public class Quotation
{
    /// <summary>
    ///     Gets or sets the unique identifier for the quotation.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    ///     Gets or sets the ID of the customer associated with this quotation.
    /// </summary>
    public int CustomerId { get; set; }

    /// <summary>
    ///     Gets or sets the origin location of the transportation job.
    /// </summary>
    public string Origin { get; set; } = string.Empty;

    /// <summary>
    ///     Gets or sets the destination location of the transportation job.
    /// </summary>
    public string Destination { get; set; } = string.Empty;

    /// <summary>
    ///     Gets or sets the type of cargo container (e.g., "20 Feet", "40 Feet").
    /// </summary>
    public string CargoType { get; set; } = string.Empty;

    /// <summary>
    ///     Gets or sets the quantity of containers for this job.
    /// </summary>
    public int ContainerQuantity { get; set; }

    /// <summary>
    ///     Gets or sets the nature of the job (e.g., "Quarantine", "Fumigation").
    /// </summary>
    public string NatureOfJob { get; set; } = string.Empty;

    /// <summary>
    ///     Gets or sets the date for the transportation.
    /// </summary>
    public DateTime TransportationDate { get; set; }

    /// <summary>
    ///     Gets or sets the current status of the quotation (e.g., "Pending", "Accepted", "Rejected").
    /// </summary>
    public string Status { get; set; } = "Pending";

    /// <summary>
    ///     Gets or sets the initial total amount before discounts are applied.
    /// </summary>
    public decimal TotalAmount { get; set; }

    /// <summary>
    ///     Gets or sets the discount rate applied to the total amount (e.g., 0.05 for 5%).
    /// </summary>
    public decimal Discount { get; set; }

    /// <summary>
    ///     Gets or sets the final amount after discounts are applied.
    /// </summary>
    public decimal FinalAmount { get; set; }

    /// <summary>
    ///     Gets or sets any additional requirements specified for this quotation.
    /// </summary>
    public string? AdditionalRequirements { get; set; }

    /// <summary>
    ///     Gets or sets the customer associated with this quotation.
    /// </summary>
    [ForeignKey("CustomerId")]
    public Customer? Customer { get; set; }

    /// <summary>
    ///     Calculates the initial amount based on the type and quantity of containers.
    ///     Sets <see cref="TotalAmount" /> and initializes <see cref="FinalAmount" /> to this value.
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
    ///     Applies a discount based on predefined business rules and updates <see cref="FinalAmount" />.
    /// </summary>
    public void ApplyDiscount()
    {
        // Discount logic based on container quantity and job type
        if (ContainerQuantity > 10 && (NatureOfJob.Contains("Quarantine") || NatureOfJob.Contains("Fumigation")))
            Discount = 0.10m; // 10% discount
        else if (ContainerQuantity > 5 && (NatureOfJob.Contains("Quarantine") || NatureOfJob.Contains("Fumigation")))
            Discount = 0.05m; // 5% discount
        else if (ContainerQuantity > 5 && NatureOfJob.Contains("Quarantine") && NatureOfJob.Contains("Fumigation"))
            Discount = 0.025m; // 2.5% discount
        else
            Discount = 0m; // No discount

        // Update FinalAmount after applying the discount
        FinalAmount = TotalAmount * (1 - Discount);
    }
}