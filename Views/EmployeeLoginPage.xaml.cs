using System.Linq;
using System.Windows;
using System.Windows.Controls;
using InterportCargoWPF.Database;
using BCrypt.Net;

namespace InterportCargoWPF.Views;

public partial class EmployeeLoginPage : Page
{
    public EmployeeLoginPage()
    {
        InitializeComponent();
    }

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
                var employee = context.Employees.SingleOrDefault(emp => emp.Email == email);

                if (employee != null && BCrypt.Net.BCrypt.Verify(password, employee.PasswordHash))
                {
                    MainWindow.Instance.ShowLoginSuccessMessage();
                    // MessageBox.Show("Login successful!");
                    MainWindow.Instance.MainFrame.Navigate(new EmployeeDashboardPage());
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
            MessageBox.Show($"An error occurred during login: {ex.Message}", "Error");
        }
    }

    private void OpenEmployeeRegisterPage_Click(object sender, RoutedEventArgs e)
    {
        MainWindow.Instance.MainFrame.Navigate(new EmployeeRegisterPage());
    }

    private void OpenCustomerLoginPage_Click(object sender, RoutedEventArgs e)
    {
        MainWindow.Instance.MainFrame.Visibility = Visibility.Collapsed;
        MainWindow.Instance.LoginForm.Visibility = Visibility.Visible;
        MainWindow.Instance.EmployeeLoginButton.Visibility = Visibility.Visible;
        MainWindow.Instance.BackButton.Visibility = Visibility.Collapsed;
        MainWindow.Instance.LogoutButton.Visibility = Visibility.Collapsed;
    }
}