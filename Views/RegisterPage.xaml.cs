using System.Linq;
using System.Windows;
using System.Windows.Controls;
using BCrypt.Net;
using InterportCargoWPF.Database;
using InterportCargoWPF.Models;

namespace InterportCargoWPF.Views;

public partial class RegisterPage : Page
{
    public RegisterPage()
    {
        InitializeComponent();
    }

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

        // Validate inputs
        if (string.IsNullOrWhiteSpace(firstName) || string.IsNullOrWhiteSpace(lastName) ||
            string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(phoneNumber) ||
            string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(address))
        {
            MessageBox.Show("All fields except Company Name are required.");
            return;
        }

        // Check email format (basic validation)
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

            // Hash the password
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);

            // Create a new Customer object
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

            // Add and save the new customer to the database
            context.Customers.Add(newCustomer);
            context.SaveChanges();

            MessageBox.Show("Registration successful!");

            // Navigate back to the login page
            ReturnToLogin_Click(this, new RoutedEventArgs());
        }
    }

    private void ReturnToLogin_Click(object sender, RoutedEventArgs e)
    {
        if (Window.GetWindow(this) is MainWindow mainWindow)
        {
            mainWindow.MainFrame.Visibility = Visibility.Collapsed;
            mainWindow.LoginForm.Visibility = Visibility.Visible;
        }
    }

    // Simple email validation method
    private bool IsValidEmail(string email)
    {
        return email.Contains("@") && email.Contains(".");
    }
}