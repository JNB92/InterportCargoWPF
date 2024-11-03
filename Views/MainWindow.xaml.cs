using System.Windows;
using System.Windows.Controls;
using InterportCargoWPF.Database;
using InterportCargoWPF.Views;

namespace InterportCargoWPF.Views
{
    public partial class MainWindow : Window
    {
        public static MainWindow Instance { get; private set; }

        public MainWindow()
        {
            InitializeComponent();
            Instance = this;
        }

        private void OpenEmployeeLoginPage_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Visibility = Visibility.Visible;
            LoginForm.Visibility = Visibility.Collapsed;
            MainFrame.Navigate(new EmployeeLoginPage()); // Navigates to EmployeeLoginPage
        }

        private void OpenRegisterWindow_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Visibility = Visibility.Visible;
            LoginForm.Visibility = Visibility.Collapsed;
            MainFrame.Navigate(new RegisterPage()); // Navigates to RegisterPage
        }

        public void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string email = EmailBox.Text;
            string enteredPassword = PasswordBox.Password;

            using (var context = new AppDbContext())
            {
                // First, check if the customer exists in the database
                var customer = context.Customers.FirstOrDefault(c => c.Email == email);

                if (customer != null)
                {
                    // If the customer exists, verify the entered password against the stored hashed password
                    bool isPasswordCorrect = BCrypt.Net.BCrypt.Verify(enteredPassword, customer.Password);

                    if (isPasswordCorrect)
                    {
                        MessageBox.Show($"Login successful! Welcome, {customer.FirstName}.");
                        
                        // Set the LoggedInCustomerId in SessionManager
                        SessionManager.LoggedInCustomerId = customer.Id;

                        // Navigate to the landing page on successful login
                        LoginSuccessful();
                    }
                    else
                    {
                        MessageBox.Show("Invalid password.");
                    }
                }
                else
                {
                    MessageBox.Show("Invalid email or password.");
                }
            }
        }

        public void LoginSuccessful()
        {
            LoginForm.Visibility = Visibility.Collapsed;
            MainFrame.Visibility = Visibility.Visible;
            MainFrame.Navigate(new LandingPage()); 
        }
        
    }
}
