using System.Windows;
using System.Windows.Controls;
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

            // Sample validation (Replace with your actual database validation)
            if (email == "admin" && enteredPassword == "password") 
            {
                MessageBox.Show("Login successful!");
                LoginSuccessful();
            }
            else
            {
                MessageBox.Show("Invalid email or password.");
            }
        }

        public void LoginSuccessful()
        {
            LoginForm.Visibility = Visibility.Collapsed;
            MainFrame.Visibility = Visibility.Visible;

            // Navigate to a landing page or dashboard after successful login
            MainFrame.Navigate(new LandingPage()); // Adjust LandingPage to your needs
        }

        // Optional method for showing the login form again if needed
        public void ShowLoginForm()
        {
            MainFrame.Visibility = Visibility.Collapsed;
            LoginForm.Visibility = Visibility.Visible;
        }
    }
}
