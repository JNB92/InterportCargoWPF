using System.Windows;
using System.Windows.Controls;
using InterportCargoWPF.Database;
using InterportCargoWPF.Models;
using BCrypt.Net;

namespace InterportCargoWPF.Views;

public partial class EmployeeRegisterPage : Page
{
    public EmployeeRegisterPage()
    {
        InitializeComponent();
    }

    private void RegisterButton_Click(object sender, RoutedEventArgs e)
    {
        var employeeType = EmployeeTypeBox.Text; // Capture EmployeeType from ComboBox
        var firstName = FirstNameBox.Text;
        var lastName = LastNameBox.Text;
        var phoneNumber = PhoneNumberBox.Text;
        var email = EmailBox.Text;
        var password = PasswordBox.Password;
        var address = AddressBox.Text;

        // Check that all required fields are filled
        if (string.IsNullOrWhiteSpace(firstName) || string.IsNullOrWhiteSpace(lastName) ||
            string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(phoneNumber) ||
            string.IsNullOrWhiteSpace(address) || string.IsNullOrWhiteSpace(employeeType) ||
            string.IsNullOrWhiteSpace(password))
        {
            MessageBox.Show("All fields are required.");
            return;
        }

        // Hash the password
        var hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);

        // Create an Employee object with all required fields
        var newEmployee = new Employee
        {
            EmployeeType = employeeType, // Assign EmployeeType
            FirstName = firstName,
            LastName = lastName,
            PhoneNumber = phoneNumber, // Assign PhoneNumber
            Email = email,
            Address = address,
            PasswordHash = hashedPassword
        };

        // Save the new employee to the database
        using (var context = new AppDbContext())
        {
            var existingEmployee = context.Employees.SingleOrDefault(emp => emp.Email == email);
            if (existingEmployee == null)
            {
                context.Employees.Add(newEmployee);
                context.SaveChanges();

                MessageBox.Show("Employee registration successful!");

                // Redirect to login or another page as needed
                MainWindow.Instance.MainFrame.Navigate(new EmployeeLoginPage());
            }
            else
            {
                MessageBox.Show("An employee with this email already exists.");
            }
        }
    }

    private void ReturnToLogin_Click(object sender, RoutedEventArgs e)
    {
        NavigationService?.Navigate(new EmployeeLoginPage());
    }
}