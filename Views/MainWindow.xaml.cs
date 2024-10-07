using System.Windows;
using System.Windows.Controls;
using System.Linq;  // Don't forget to include this for LINQ queries
using InterportCargoWPF.Database;
using InterportCargoWPF.Models;


namespace InterportCargoWPF.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static MainWindow Instance { get; private set; }
        public MainWindow()
        {
            InitializeComponent();
            Instance = this;
        }

        private void OpenRegisterWindow_Click(object sender, RoutedEventArgs e)
        {
            RegisterWindow registerWindow = new RegisterWindow();
            registerWindow.Show();
            this.Close();
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
            // Hide the login form and show the frame
            LoginForm.Visibility = Visibility.Collapsed;
            MainFrame.Visibility = Visibility.Visible;

            // Navigate to the landing page on successful login
            MainFrame.NavigationService.Navigate(new LandingPage());
        }
    }
}
