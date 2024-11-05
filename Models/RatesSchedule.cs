namespace InterportCargoWPF.Models
{
    /// <summary>
    /// Represents a rate schedule for various fees associated with container handling and transportation.
    /// Includes rates for different container types and associated service charges.
    /// </summary>
    public class RateSchedule
    {
        /// <summary>
        /// Gets or sets the unique identifier for the rate schedule entry.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the type of rate (e.g., "Container Handling", "Fumigation").
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Gets or sets the rate for a 20-feet container.
        /// </summary>
        public decimal TwentyFeetContainer { get; set; }

        /// <summary>
        /// Gets or sets the rate for a 40-feet container.
        /// </summary>
        public decimal FortyFeetContainer { get; set; }

        /// <summary>
        /// Gets or sets the depot charges associated with this rate type.
        /// </summary>
        public decimal DepotCharges { get; set; }

        /// <summary>
        /// Gets or sets the charges for LCL (Less than Container Load) delivery.
        /// </summary>
        public decimal LclDeliveryCharges { get; set; }

        /// <summary>
        /// Gets or sets the wharf booking fee.
        /// </summary>
        public decimal WharfBookingFee { get; set; }

        /// <summary>
        /// Gets or sets the lift-on/lift-off fee.
        /// </summary>
        public decimal LiftOnLiftOffFee { get; set; }

        /// <summary>
        /// Gets or sets the fumigation fee.
        /// </summary>
        public decimal FumigationFee { get; set; }

        /// <summary>
        /// Gets or sets the tailgate inspection fee.
        /// </summary>
        public decimal TailgateInspectionFee { get; set; }

        /// <summary>
        /// Gets or sets the storage fee.
        /// </summary>
        public decimal StorageFee { get; set; }

        /// <summary>
        /// Gets or sets the facility fee.
        /// </summary>
        public decimal FacilityFee { get; set; }

        /// <summary>
        /// Gets or sets the wharf inspection fee.
        /// </summary>
        public decimal WharfInspectionFee { get; set; }

        /// <summary>
        /// Gets or sets the GST (Goods and Services Tax) rate.
        /// Default is set to 10%.
        /// </summary>
        public decimal GstRate { get; set; } = 0.10m;
    }
}
