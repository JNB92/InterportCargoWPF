namespace InterportCargoWPF.Models
{
    /// <summary>
    /// Represents an employee in the application, containing personal details, contact information, and login credentials.
    /// </summary>
    public class Employee
    {
        /// <summary>
        /// Gets or sets the unique identifier for the employee.
        /// </summary>
        public string EmployeeID { get; set; }

        /// <summary>
        /// Gets or sets the first name of the employee.
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets the last name of the employee.
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Gets or sets the email address of the employee.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the phone number of the employee.
        /// </summary>
        public string PhoneNumber { get; set; }

        /// <summary>
        /// Gets or sets the type of the employee (e.g., "Manager", "Staff").
        /// </summary>
        public string EmployeeType { get; set; }

        /// <summary>
        /// Gets or sets the physical address of the employee.
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// Gets or sets the hashed password of the employee for authentication purposes.
        /// </summary>
        public string PasswordHash { get; set; }
    }
}