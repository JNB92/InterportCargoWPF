using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using InterportCargoWPF.Database;

namespace InterportCargoWPF.Views
{
    /// <summary>
    /// Represents the main window of the application, providing navigation and login functionality for employees and customers.
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Gets the singleton instance of the <see cref="MainWindow"/>.
        /// </summary>
        public static MainWindow Instance { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindow"/> class and sets up the singleton instance.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            Instance = this;
        }

        /// <summary>
        /// Opens the employee login page within the main frame.
        /// </summary>
        private void OpenEmployeeLoginPage_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Visibility = Visibility.Visible;
            LoginForm.Visibility = Visibility.Collapsed;
            MainFrame.Navigate(new EmployeeLoginPage());
        }

        /// <summary>
        /// Displays a login success message for a short duration.
        /// </summary>
        public void ShowLoginSuccessMessage()
        {
            LoginSuccessTextBlock.Visibility = Visibility.Visible;

            // Set up a timer to hide the message after 2 seconds
            var timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(2)
            };
            timer.Tick += (s, args) =>
            {
                LoginSuccessTextBlock.Visibility = Visibility.Collapsed;
                timer.Stop();
            };
            timer.Start();
        }

        /// <summary>
        /// Opens the employee registration page within the main frame.
        /// </summary>
        private void OpenRegisterWindow_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Visibility = Visibility.Visible;
            LoginForm.Visibility = Visibility.Collapsed;
            MainFrame.Navigate(new RegisterPage());
        }

        /// <summary>
        /// Handles the customer login process, verifying credentials and navigating to the landing page upon success.
        /// </summary>
        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            var email = EmailBox.Text;
            var enteredPassword = PasswordBox.Password;

            using (var context = new AppDbContext())
            {
                // Check if the customer exists in the database
                var customer = context.Customers.FirstOrDefault(c => c.Email == email);

                if (customer != null)
                {
                    // Verify the entered password
                    var isPasswordCorrect = BCrypt.Net.BCrypt.Verify(enteredPassword, customer.Password);

                    if (isPasswordCorrect)
                    {
                        SessionManager.LoggedInCustomerId = customer.Id;
                        LoginSuccessful();
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

        /// <summary>
        /// Displays a message indicating that the back navigation is not possible.
        /// </summary>
        public void ShowCantGoBackMessage()
        {
            CantgobackTextBlock.Visibility = Visibility.Visible;

            // Set up a timer to hide the message after 5 seconds
            var timer = new DispatcherTimer
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

        /// <summary>
        /// Handles the back navigation button click event, going back in the navigation history or showing a message if not possible.
        /// </summary>
        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            if (MainFrame.CanGoBack && MainFrame.Content != this && MainFrame.Content is not EmployeeDashboardPage)
            {
                MainFrame.GoBack();
            }
            else
            {
                ShowCantGoBackMessage();
            }
        }

        /// <summary>
        /// Finalizes login actions by navigating to the landing page and updating the visibility of navigation buttons.
        /// </summary>
        public void LoginSuccessful()
        {
            LoginForm.Visibility = Visibility.Collapsed;
            MainFrame.Visibility = Visibility.Visible;
            MainFrame.Navigate(new LandingPage());

            EmployeeLoginButton.Visibility = Visibility.Collapsed;
            BackButton.Visibility = Visibility.Visible;
            LogoutButton.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// Logs out the user, clears credentials, and displays the login form.
        /// </summary>
        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            EmailBox.Text = string.Empty;
            PasswordBox.Password = string.Empty;

            LogoutMessageTextBlock.Text = "You have been logged out.";
            LogoutMessageTextBlock.Visibility = Visibility.Visible;

            LoginForm.Visibility = Visibility.Visible;
            MainFrame.Visibility = Visibility.Collapsed;

            EmployeeLoginButton.Visibility = Visibility.Visible;
            BackButton.Visibility = Visibility.Collapsed;
            LogoutButton.Visibility = Visibility.Collapsed;
        }

        /// <summary>
        /// Clears the logout message when the user starts entering credentials.
        /// </summary>
        private void ClearLogoutMessage(object sender, RoutedEventArgs e)
        {
            LogoutMessageTextBlock.Visibility = Visibility.Collapsed;
        }
    }
}
