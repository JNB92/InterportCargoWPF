using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace InterportCargoWPF.Tests
{
    // Production class for user management
    public class UserService
    {
        private readonly List<User> _users = new List<User>();
        private readonly List<Quotation> _quotations = new List<Quotation>();

        /// <summary>
        /// Registers a new user with the provided details.
        /// </summary>
        /// <param name="firstName">The first name of the user.</param>
        /// <param name="lastName">The last name of the user.</param>
        /// <param name="email">The email address of the user.</param>
        /// <param name="phoneNumber">The phone number of the user.</param>
        /// <param name="password">The password for the user account.</param>
        /// <returns>
        /// The ID of the newly registered user if successful, -1 if input is invalid, 
        /// or -2 if the email already exists.
        /// </returns>
        public int Register(string firstName, string lastName, string email, string phoneNumber, string password)
        {
            // Basic validation
            if (string.IsNullOrWhiteSpace(firstName) || string.IsNullOrWhiteSpace(lastName) ||
                string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                return -1; // Invalid input
            }

            // Check if the user already exists
            if (_users.Any(u => u.Email == email))
            {
                return -2; // User already exists
            }

            var newUser = new User
            {
                Id = _users.Count + 1,
                FirstName = firstName,
                LastName = lastName,
                Email = email,
                PhoneNumber = phoneNumber,
                Password = BCrypt.Net.BCrypt.HashPassword(password) // Password hashing
            };

            _users.Add(newUser);
            return newUser.Id; // Return new user ID
        }

        /// <summary>
        /// Logs in a user with the provided email and password.
        /// </summary>
        /// <param name="email">The email address of the user.</param>
        /// <param name="password">The password for the user account.</param>
        /// <returns>
        /// The ID of the user if login is successful, 
        /// 0 if the user is not found, or 
        /// -1 if the password is incorrect.
        /// </returns>
        public int Login(string email, string password)
        {
            var user = _users.FirstOrDefault(u => u.Email == email);
            if (user == null) return 0; // User not found

            if (BCrypt.Net.BCrypt.Verify(password, user.Password))
            {
                SessionManager.LoggedInCustomerId = user.Id; // Successful login
                return user.Id;
            }

            return -1; // Invalid password
        }

        /// <summary>
        /// Logs out the currently logged-in user.
        /// </summary>
        public void Logout()
        {
            SessionManager.LoggedInCustomerId = 0; // Clear the session
        }

        /// <summary>
        /// Submits a quotation request with the provided details.
        /// </summary>
        /// <param name="customerId">The ID of the customer submitting the quotation.</param>
        /// <param name="details">The details of the quotation.</param>
        /// <returns>The ID of the created quotation.</returns>
        public int SubmitQuotation(int customerId, string details)
        {
            var newQuotation = new Quotation
            {
                Id = _quotations.Count + 1,
                CustomerId = customerId,
                Details = details,
                Status = "Pending"
            };

            _quotations.Add(newQuotation);
            return newQuotation.Id; // Return new quotation ID
        }

        /// <summary>
        /// Gets all quotations submitted by customers.
        /// </summary>
        /// <returns>A list of all quotations.</returns>
        public IEnumerable<Quotation> GetAllQuotations() => _quotations;
    }

    /// <summary>
    /// Represents a user in the system.
    /// </summary>
    public class User
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Password { get; set; }
    }

    /// <summary>
    /// Represents a quotation submitted by a user.
    /// </summary>
    public class Quotation
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public string Details { get; set; }
        public string Status { get; set; } // e.g., Pending, Accepted, Rejected
    }

    // Unit tests for UserService
    [TestFixture]
    public class UserServiceTests
    {
        private UserService _userService;

        [SetUp]
        public void Setup()
        {
            _userService = new UserService(); // Instantiate UserService
        }

        /// <summary>
        /// Tests that registering a valid user returns the expected user ID.
        /// </summary>
        [Test]
        public void Register_ValidDetails_ReturnsUserId()
        {
            // Act
            var userId = _userService.Register("John", "Doe", "john@example.com", "123456789", "password123");

            // Assert
            Assert.That(userId, Is.EqualTo(1)); // Expect the first user ID to be 1
        }

        /// <summary>
        /// Tests that attempting to register with a duplicate email returns -2.
        /// </summary>
        [Test]
        public void Register_DuplicateEmail_ReturnsNegativeTwo()
        {
            // Arrange
            _userService.Register("John", "Doe", "john@example.com", "123456789", "password123");

            // Act
            var result = _userService.Register("Jane", "Doe", "john@example.com", "987654321", "password456");

            // Assert
            Assert.That(result, Is.EqualTo(-2)); // User already exists
        }

        /// <summary>
        /// Tests that registering a user with invalid details returns -1.
        /// </summary>
        [Test]
        public void Register_InvalidDetails_ReturnsNegativeOne()
        {
            // Act
            var result = _userService.Register("", "Doe", "john@example.com", "123456789", "password123");

            // Assert
            Assert.That(result, Is.EqualTo(-1)); // Invalid input
        }

        /// <summary>
        /// Tests that logging in with valid credentials returns the expected user ID.
        /// </summary>
        [Test]
        public void Login_ValidCredentials_ReturnsUserId()
        {
            // Arrange
            _userService.Register("John", "Doe", "john@example.com", "123456789", "password123");

            // Act
            var result = _userService.Login("john@example.com", "password123");

            // Assert
            Assert.That(result, Is.EqualTo(1)); // Customer ID should be returned
        }

        /// <summary>
        /// Tests that logging in with an invalid password returns -1.
        /// </summary>
        [Test]
        public void Login_InvalidPassword_ReturnsNegativeOne()
        {
            // Arrange
            _userService.Register("John", "Doe", "john@example.com", "123456789", "password123");

            // Act
            var result = _userService.Login("john@example.com", "wrongpassword");

            // Assert
            Assert.That(result, Is.EqualTo(-1)); // Should return -1 for invalid password
        }

        /// <summary>
        /// Tests that logging in with a non-existent user returns 0.
        /// </summary>
        [Test]
        public void Login_NonExistentUser_ReturnsZero()
        {
            // Act
            var result = _userService.Login("nonexistent@example.com", "password123");

            // Assert
            Assert.That(result, Is.EqualTo(0)); // Should return 0 for non-existent user
        }

        /// <summary>
        /// Tests that logging out resets the logged-in user ID.
        /// </summary>
        [Test]
        public void Logout_ResetsLoggedInUser()
        {
            // Arrange
            _userService.Register("John", "Doe", "john@example.com", "123456789", "password123");
            _userService.Login("john@example.com", "password123");

            // Act
            _userService.Logout();

            // Assert
            Assert.That(SessionManager.LoggedInCustomerId, Is.EqualTo(0)); // User should be logged out
        }

        /// <summary>
        /// Tests that submitting a quotation returns the correct quotation ID.
        /// </summary>
        [Test]
        public void SubmitQuotation_ValidDetails_ReturnsQuotationId()
        {
            // Arrange
            var customerId = _userService.Register("John", "Doe", "john@example.com", "123456789", "password123");
            var details = "Quotation for shipping goods.";

            // Act
            var quotationId = _userService.SubmitQuotation(customerId, details);

            // Assert
            Assert.That(quotationId, Is.EqualTo(1)); // Expect the first quotation ID to be 1
        }

        /// <summary>
        /// Tests that retrieving all quotations returns the expected count.
        /// </summary>
        [Test]
        public void GetAllQuotations_ReturnsAllSubmittedQuotations()
        {
            // Arrange
            var customerId = _userService.Register("John", "Doe", "john@example.com", "123456789", "password123");
            _userService.SubmitQuotation(customerId, "First quotation.");
            _userService.SubmitQuotation(customerId, "Second quotation.");

            // Act
            var quotations = _userService.GetAllQuotations();

            // Assert
            Assert.That(quotations.Count(), Is.EqualTo(2)); // Should return 2 quotations
        }
    }
}
