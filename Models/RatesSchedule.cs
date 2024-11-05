namespace InterportCargoWPF.Models;

public class RateSchedule
{
    public int Id { get; set; }
    public string Type { get; set; } // or ContainerType, choose one
    public decimal TwentyFeetContainer { get; set; }
    public decimal FortyFeetContainer { get; set; }
    public decimal DepotCharges { get; set; }
    public decimal LclDeliveryCharges { get; set; }

    // Additional properties
    public decimal WharfBookingFee { get; set; }
    public decimal LiftOnLiftOffFee { get; set; }
    public decimal FumigationFee { get; set; }
    public decimal TailgateInspectionFee { get; set; }
    public decimal StorageFee { get; set; }
    public decimal FacilityFee { get; set; }
    public decimal WharfInspectionFee { get; set; }
    public decimal GstRate { get; set; } = 0.10m; // Default 10%
}