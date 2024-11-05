using System.Linq;
using System.Windows;
using System.Windows.Controls;
using InterportCargoWPF.Database;
using InterportCargoWPF.Models;
using BCrypt.Net;

namespace InterportCargoWPF.Views
{
    /// <summary>
    /// Represents the employee registration page, allowing new employees to create an account.
    /// </summary>
    public partial class EmployeeRegisterPage : Page
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EmployeeRegisterPage"/> class.
        /// </summary>
        public EmployeeRegisterPage()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Handles the registration button click event, collecting input fields, validating them, 
        /// and registering a new employee in the database if all requirements are met.
        /// </summary>
        /// <param name="sender">The button that was clicked.</param>
        /// <param name="e">The event data.</param>
        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            var employeeType = EmployeeTypeBox.Text;
            var firstName = FirstNameBox.Text;
            var lastName = LastNameBox.Text;
            var phoneNumber = PhoneNumberBox.Text;
            var email = EmailBox.Text;
            var password = PasswordBox.Password;
            var address = AddressBox.Text;

            // Validate that all required fields have been filled
            if (string.IsNullOrWhiteSpace(firstName) || string.IsNullOrWhiteSpace(lastName) ||
                string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(phoneNumber) ||
                string.IsNullOrWhiteSpace(address) || string.IsNullOrWhiteSpace(employeeType) ||
                string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("All fields are required.");
                return;
            }

            // Hash the password for secure storage
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);

            // Create a new employee with the input values
            var newEmployee = new Employee
            {
                EmployeeType = employeeType,
                FirstName = firstName,
                LastName = lastName,
                PhoneNumber = phoneNumber,
                Email = email,
                Address = address,
                PasswordHash = hashedPassword
            };

            // Save the new employee to the database, checking if an employee with the same email already exists
            using (var context = new AppDbContext())
            {
                var existingEmployee = context.Employees.SingleOrDefault(emp => emp.Email == email);
                if (existingEmployee == null)
                {
                    context.Employees.Add(newEmployee);
                    context.SaveChanges();

                    MessageBox.Show("Employee registration successful!");

                    // Navigate to the login page
                    MainWindow.Instance.MainFrame.Navigate(new EmployeeLoginPage());
                }
                else
                {
                    MessageBox.Show("An employee with this email already exists.");
                }
            }
        }

        /// <summary>
        /// Returns the user to the login page when the cancel button is clicked.
        /// </summary>
        /// <param name="sender">The button that was clicked.</param>
        /// <param name="e">The event data.</param>
        private void ReturnToLogin_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new EmployeeLoginPage());
        }
    }
}
