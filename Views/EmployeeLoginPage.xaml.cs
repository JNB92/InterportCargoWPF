using System.Linq;
using System.Windows;
using System.Windows.Controls;
using InterportCargoWPF.Database;
using BCrypt.Net;

namespace InterportCargoWPF.Views
{
    public partial class EmployeeLoginPage : Page
    {
        public EmployeeLoginPage()
        {
            InitializeComponent();
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string email = EmailBox.Text;
            string password = PasswordBox.Password;

            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Please enter both email and password.");
                return;
            }

            using (var context = new AppDbContext())
            {
                var employee = context.Employees.SingleOrDefault(emp => emp.Email == email);

                if (employee != null && BCrypt.Net.BCrypt.Verify(password, employee.PasswordHash))
                {
                    MessageBox.Show("Login successful!");

                    // Navigate to the employee dashboard or main application section for employees
                    MainWindow.Instance.MainFrame.Navigate(new EmployeeDashboardPage());

                    // Update button visibility on the MainWindow after successful login
                    MainWindow.Instance.EmployeeLoginButton.Visibility = Visibility.Collapsed;
                    MainWindow.Instance.BackButton.Visibility = Visibility.Visible;
                    MainWindow.Instance.LogoutButton.Visibility = Visibility.Visible;
                }
                else
                {
                    MessageBox.Show("Invalid email or password.");
                }
            }
        }

        private void OpenEmployeeRegisterPage_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.Instance.MainFrame.Visibility = Visibility.Visible;
            MainWindow.Instance.LoginForm.Visibility = Visibility.Collapsed;
            MainWindow.Instance.MainFrame.Navigate(new EmployeeRegisterPage());
        }

        private void OpenCustomerLoginPage_Click(object sender, RoutedEventArgs e)
        {
            // Reset the interface to Customer Login
            MainWindow.Instance.MainFrame.Visibility = Visibility.Collapsed;
            MainWindow.Instance.LoginForm.Visibility = Visibility.Visible;

            // Ensure buttons are in initial state for Customer Login
            MainWindow.Instance.EmployeeLoginButton.Visibility = Visibility.Visible;
            MainWindow.Instance.BackButton.Visibility = Visibility.Collapsed;
            MainWindow.Instance.LogoutButton.Visibility = Visibility.Collapsed;
        }
    }
}
