using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
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
            // Show the Employee Login page within the Frame
            MainFrame.Visibility = Visibility.Visible;
            LoginForm.Visibility = Visibility.Collapsed;
            MainFrame.Navigate(new EmployeeLoginPage());

            // Do not alter the button visibility here; only handle it after successful login.
        }
        public void ShowLoginSuccessMessage()
        {
            LoginSuccessTextBlock.Visibility = Visibility.Visible;

            // Set up a DispatcherTimer to hide the message after 5 seconds
            DispatcherTimer timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(5)
            };
            timer.Tick += (s, args) =>
            {
                LoginSuccessTextBlock.Visibility = Visibility.Collapsed;
                timer.Stop();
            };
            timer.Start();
        }


        private void OpenRegisterWindow_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Visibility = Visibility.Visible;
            LoginForm.Visibility = Visibility.Collapsed;
            MainFrame.Navigate(new RegisterPage());
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
                    // Verify the entered password against the stored hashed password
                    bool isPasswordCorrect = BCrypt.Net.BCrypt.Verify(enteredPassword, customer.Password);

                    if (isPasswordCorrect)
                    {
                        //MessageBox.Show($"Login successful! Welcome, {customer.FirstName} {customer.LastName}.");
                
                        // Set the LoggedInCustomerId in SessionManager
                        SessionManager.LoggedInCustomerId = customer.Id;

                        // Navigate to the landing page on successful login
                        LoginSuccessful();

                        // Show the universal "Login Successful" message
                        ShowLoginSuccessMessage();
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

        public void ShowCantGoBackMessage()
        {
            CantgobackTextBlock.Visibility = Visibility.Visible;

            // Set up a DispatcherTimer to hide the message after 5 seconds
            DispatcherTimer timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(5)
            };
            timer.Tick += (s, args) =>
            {
                CantgobackTextBlock.Visibility = Visibility.Collapsed;
                timer.Stop();
            };
            timer.Start();
        }


        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            if (MainFrame.CanGoBack && MainFrame.Content != this && MainFrame.Content is not EmployeeDashboardPage)
            {
                MainFrame.GoBack();
            }
            else
            {
                // Show the "can't go back" message on any page
                ShowCantGoBackMessage();
            }
        }


        

        public void LoginSuccessful()
        {
            LoginForm.Visibility = Visibility.Collapsed;
            MainFrame.Visibility = Visibility.Visible;
            MainFrame.Navigate(new LandingPage());

            // Show Back and Logout buttons, hide Employee Login button after successful login
            EmployeeLoginButton.Visibility = Visibility.Collapsed;
            BackButton.Visibility = Visibility.Visible;
            LogoutButton.Visibility = Visibility.Visible;
        }

        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            EmailBox.Text = string.Empty;
            PasswordBox.Password = string.Empty;
            
            // Display the logout message
            LogoutMessageTextBlock.Text = "You have been logged out.";
            LogoutMessageTextBlock.Visibility = Visibility.Visible;

            // Reset to the login form and update button visibility
            LoginForm.Visibility = Visibility.Visible;
            MainFrame.Visibility = Visibility.Collapsed;

            EmployeeLoginButton.Visibility = Visibility.Visible;
            BackButton.Visibility = Visibility.Collapsed;
            LogoutButton.Visibility = Visibility.Collapsed;
        }
        private void ClearLogoutMessage(object sender, RoutedEventArgs e)
        {
            // Hide the logout message when the user starts entering credentials
            LogoutMessageTextBlock.Visibility = Visibility.Collapsed;
        }
    }
}
