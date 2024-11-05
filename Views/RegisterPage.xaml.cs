using System.Windows;
using System.Windows.Controls;
using InterportCargoWPF.Database;
using InterportCargoWPF.Models;

namespace InterportCargoWPF.Views;

/// <summary>
///     Represents the page for new customer registration, allowing users to enter details and create an account.
/// </summary>
public partial class RegisterPage : Page
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="RegisterPage" /> class.
    /// </summary>
    public RegisterPage()
    {
        InitializeComponent();
    }

    /// <summary>
    ///     Handles the click event for the Register button, validating input, creating a new customer,
    ///     and saving the customer data to the database.
    /// </summary>
    private void RegisterButton_Click(object sender, RoutedEventArgs e)
    {
        // Retrieve input values
        var firstName = FirstNameBox.Text;
        var lastName = LastNameBox.Text;
        var email = EmailBox.Text;
        var phoneNumber = PhoneNumberBox.Text;
        var password = PasswordBox.Password;
        var companyName = CompanyNameBox.Text;
        var address = AddressBox.Text;

        // Validate required fields
        if (string.IsNullOrWhiteSpace(firstName) || string.IsNullOrWhiteSpace(lastName) ||
            string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(phoneNumber) ||
            string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(address))
        {
            MessageBox.Show("All fields except Company Name are required.");
            return;
        }

        // Basic email format validation
        if (!IsValidEmail(email))
        {
            MessageBox.Show("Please enter a valid email address.");
            return;
        }

        using (var context = new AppDbContext())
        {
            // Check if email is already registered
            if (context.Customers.Any(c => c.Email == email))
            {
                MessageBox.Show("A customer with this email already exists.");
                return;
            }

            // Hash the password for secure storage
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);

            // Create a new Customer object with the input data
            var newCustomer = new Customer
            {
                FirstName = firstName,
                LastName = lastName,
                Email = email,
                PhoneNumber = phoneNumber,
                Password = hashedPassword,
                Company = string.IsNullOrWhiteSpace(companyName) ? null : companyName,
                Address = address
            };

            // Add the new customer to the database and save changes
            context.Customers.Add(newCustomer);
            context.SaveChanges();

            MessageBox.Show("Registration successful!");

            // Navigate back to the login page
            ReturnToLogin_Click(this, new RoutedEventArgs());
        }
    }

    /// <summary>
    ///     Navigates the user back to the login page after a successful registration.
    /// </summary>
    private void ReturnToLogin_Click(object sender, RoutedEventArgs e)
    {
        if (Window.GetWindow(this) is MainWindow mainWindow)
        {
            mainWindow.MainFrame.Visibility = Visibility.Collapsed;
            mainWindow.LoginForm.Visibility = Visibility.Visible;
        }
    }

    /// <summary>
    ///     Validates the format of the provided email address.
    /// </summary>
    /// <param name="email">The email address to validate.</param>
    /// <returns>True if the email format is valid, otherwise false.</returns>
    private bool IsValidEmail(string email)
    {
        return email.Contains("@") && email.Contains(".");
    }
}