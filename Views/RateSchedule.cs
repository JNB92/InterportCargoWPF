namespace InterportCargoWPF.Views;

public class RateSchedule
{
    public int Id { get; set; }
    public string ContainerType { get; set; }
    public decimal WharfBookingFee { get; set; }
    public decimal LiftOnLiftOffFee { get; set; }
    public decimal FumigationFee { get; set; }
    public decimal LclDeliveryDepot { get; set; }
    public decimal TailgateInspectionFee { get; set; }
    public decimal StorageFee { get; set; }
    public decimal FacilityFee { get; set; }
    public decimal WharfInspectionFee { get; set; }
    public decimal GstRate { get; set; } = 0.10m; // Default 10%
}
