namespace InterportCargoWPF
{
    /// <summary>
    /// Manages session-related information for the application, such as tracking the logged-in customer.
    /// </summary>
    public static class SessionManager
    {
        /// <summary>
        /// Gets or sets the ID of the currently logged-in customer.
        /// </summary>
        public static int LoggedInCustomerId { get; set; }
    }
}