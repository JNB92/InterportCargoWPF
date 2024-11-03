using System.Linq;
using System.Windows;
using System.Windows.Controls;
using InterportCargoWPF.Database;
using InterportCargoWPF.Models;
using BCrypt.Net;

namespace InterportCargoWPF.Views
{
    /// <summary>
    /// Interaction logic for RegisterPage.xaml
    /// </summary>
    public partial class RegisterPage : Page  // Inherit from Page
    {
        public RegisterPage()
        {
            InitializeComponent();
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            string firstName = FirstNameBox.Text;
            string lastName = LastNameBox.Text;
            string email = EmailBox.Text;
            string phoneNumber = PhoneNumberBox.Text;
            string password = PasswordBox.Password;

            if (string.IsNullOrWhiteSpace(firstName) || string.IsNullOrWhiteSpace(lastName) ||
                string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(phoneNumber) ||
                string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("All fields are required.");
                return;
            }

            // Hash the password
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);

            // Create a new Customer object
            var newCustomer = new Customer(firstName, lastName, email, phoneNumber, hashedPassword);

            // Save the new customer to the database (using Entity Framework)
            using (var context = new AppDbContext())
            {
                // Check if the customer already exists
                var existingCustomer = context.Customers.SingleOrDefault(c => c.Email == email);
                if (existingCustomer == null)
                {
                    context.Customers.Add(newCustomer);
                    context.SaveChanges();

                    MessageBox.Show("Registration successful!");

                    // Navigate back to login page after successful registration
                    ReturnToLogin_Click(this, new RoutedEventArgs());
                }
                else
                {
                    MessageBox.Show("A customer with this email already exists.");
                }
            }
        }

        private void ReturnToLogin_Click(object sender, RoutedEventArgs e)
        {
            // Get the window that contains this page, which is MainWindow
            if (Window.GetWindow(this) is MainWindow mainWindow)
            {
                // Hide the frame and show the login form within MainWindow
                mainWindow.MainFrame.Visibility = Visibility.Collapsed;
                mainWindow.LoginForm.Visibility = Visibility.Visible;
            }
        }

    }
}
