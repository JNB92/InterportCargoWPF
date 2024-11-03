namespace InterportCargoWPF.Models
{
    public class RateSchedule
    {
        public int Id { get; set; }
        public string ContainerType { get; set; } // "20 Feet" or "40 Feet"
        public decimal WharfBookingFee { get; set; }
        public decimal LiftOnLiftOffFee { get; set; }
        public decimal FumigationFee { get; set; }
        public decimal LclDeliveryDepot { get; set; }
        public decimal TailgateInspectionFee { get; set; }
        public decimal StorageFee { get; set; }
        public decimal FacilityFee { get; set; }
        public decimal WharfInspectionFee { get; set; }
        public decimal GstRate { get; set; } = 0.10m; // 10% by default
    }
}