namespace InterportCargoWPF.Models
{
    /// <summary>
    /// Represents a customer in the application, containing personal details, contact information, and associated quotations.
    /// </summary>
    public class Customer
    {
        /// <summary>
        /// Gets or sets the unique identifier for the customer.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the first name of the customer.
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets the last name of the customer.
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Gets or sets the email address of the customer.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the phone number of the customer.
        /// </summary>
        public string PhoneNumber { get; set; }

        /// <summary>
        /// Gets or sets the physical address of the customer.
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// Gets or sets the company name associated with the customer, if any.
        /// </summary>
        public string? Company { get; set; }

        /// <summary>
        /// Gets or sets the hashed password of the customer.
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Gets the full name of the customer, combining first and last names.
        /// </summary>
        public string FullName => $"{FirstName} {LastName}";

        /// <summary>
        /// Gets or sets the collection of quotations associated with the customer.
        /// </summary>
        public ICollection<Quotation> Quotations { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Customer"/> class with default values.
        /// </summary>
        public Customer()
        {
            Quotations = new List<Quotation>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Customer"/> class with specified details.
        /// </summary>
        /// <param name="firstName">The first name of the customer.</param>
        /// <param name="lastName">The last name of the customer.</param>
        /// <param name="email">The email address of the customer.</param>
        /// <param name="phoneNumber">The phone number of the customer.</param>
        /// <param name="address">The physical address of the customer.</param>
        /// <param name="company">The company name associated with the customer, if any.</param>
        /// <param name="password">The hashed password of the customer.</param>
        public Customer(string firstName, string lastName, string email, string phoneNumber, string address,
            string? company, string password) : this()
        {
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            PhoneNumber = phoneNumber;
            Address = address;
            Company = company;
            Password = password;
        }
    }
}
