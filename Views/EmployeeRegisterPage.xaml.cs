using System.Windows;
using System.Windows.Controls;
using InterportCargoWPF.Database;
using InterportCargoWPF.Models;
using BCrypt.Net;

namespace InterportCargoWPF.Views
{
    public partial class EmployeeRegisterPage : Page
    {
        public EmployeeRegisterPage()
        {
            InitializeComponent();
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            string employeeId = EmployeeIDBox.Text;
            string employeeType = EmployeeTypeBox.Text;  // Capture EmployeeType from ComboBox
            string firstName = FirstNameBox.Text;
            string lastName = LastNameBox.Text;
            string phoneNumber = PhoneNumberBox.Text;
            string email = EmailBox.Text;
            string password = PasswordBox.Password;

            // Check that all required fields are filled
            if (string.IsNullOrWhiteSpace(firstName) || string.IsNullOrWhiteSpace(lastName) ||
                string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(employeeId) ||
                string.IsNullOrWhiteSpace(phoneNumber) || string.IsNullOrWhiteSpace(employeeType) ||
                string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("All fields are required.");
                return;
            }

            // Hash the password
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);

            // Create an Employee object with all required fields
            var newEmployee = new Employee
            {
                EmployeeID = employeeId,
                EmployeeType = employeeType,  // Assign EmployeeType
                FirstName = firstName,
                LastName = lastName,
                PhoneNumber = phoneNumber,    // Assign PhoneNumber
                Email = email,
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
}
