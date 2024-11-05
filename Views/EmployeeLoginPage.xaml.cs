using System.Windows;
using System.Windows.Controls;
using InterportCargoWPF.Database;

namespace InterportCargoWPF.Views;

/// <summary>
///     Represents the login page for employees.
/// </summary>
public partial class EmployeeLoginPage : Page
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="EmployeeLoginPage" /> class.
    /// </summary>
    public EmployeeLoginPage()
    {
        InitializeComponent();
    }

    /// <summary>
    ///     Handles the login button click event, verifying employee credentials and navigating to the dashboard if successful.
    /// </summary>
    /// <param name="sender">The button that was clicked.</param>
    /// <param name="e">The event data.</param>
    private void LoginButton_Click(object sender, RoutedEventArgs e)
    {
        var email = EmailBox.Text;
        var password = PasswordBox.Password;

        if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
        {
            MessageBox.Show("Please enter both email and password.");
            return;
        }

        try
        {
            using (var context = new AppDbContext())
            {
                // Retrieve the employee record based on the provided email
                var employee = context.Employees.SingleOrDefault(emp => emp.Email == email);

                // Check if employee exists and if password matches the stored hash
                if (employee != null && BCrypt.Net.BCrypt.Verify(password, employee.PasswordHash))
                {
                    MainWindow.Instance.ShowLoginSuccessMessage();
                    MainWindow.Instance.MainFrame.Navigate(new EmployeeDashboardPage());

                    // Update visibility of main window elements based on login status
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
        catch (Exception ex)
        {
            MessageBox.Show($"An error occurred during login: {ex.Message}", "Error", MessageBoxButton.OK,
                MessageBoxImage.Error);
        }
    }

    /// <summary>
    ///     Navigates to the employee registration page when the register button is clicked.
    /// </summary>
    /// <param name="sender">The button that was clicked.</param>
    /// <param name="e">The event data.</param>
    private void OpenEmployeeRegisterPage_Click(object sender, RoutedEventArgs e)
    {
        MainWindow.Instance.MainFrame.Navigate(new EmployeeRegisterPage());
    }

    /// <summary>
    ///     Opens the customer login page, adjusting visibility of relevant UI elements.
    /// </summary>
    /// <param name="sender">The button that was clicked.</param>
    /// <param name="e">The event data.</param>
    private void OpenCustomerLoginPage_Click(object sender, RoutedEventArgs e)
    {
        MainWindow.Instance.MainFrame.Visibility = Visibility.Collapsed;
        MainWindow.Instance.LoginForm.Visibility = Visibility.Visible;
        MainWindow.Instance.EmployeeLoginButton.Visibility = Visibility.Visible;
        MainWindow.Instance.BackButton.Visibility = Visibility.Collapsed;
        MainWindow.Instance.LogoutButton.Visibility = Visibility.Collapsed;
    }
}