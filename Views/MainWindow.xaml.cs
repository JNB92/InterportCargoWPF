using System.Windows;
using System.Windows.Controls;
using InterportCargoWPF.Database;
using InterportCargoWPF.Models;

namespace InterportCargoWPF.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void OpenRegisterWindow_Click(object sender, RoutedEventArgs e)
        {
            RegisterWindow registerWindow = new RegisterWindow();
            registerWindow.Show();
            this.Close();
        }

        // private void OpenLoginWindow_Click(object sender, RoutedEventArgs e)
        // {
        //     LoginWindow loginWindow = new LoginWindow();
        //     loginWindow.Show();
        //     this.Close();
        // }

        public void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string email = EmailBox.Text;
            string password = PasswordBox.Password;

            using (var context = new AppDbContext())
            {
                var customer = context.Customers
                    .FirstOrDefault(c => c.Email == email && c.Password == password);

                if (customer != null)
                {
                    MessageBox.Show($"Login successful! Welcome, {customer.FirstName}.");
                    LoginSuccessful(this,new RoutedEventArgs());
                }
                else
                {
                    MessageBox.Show("Invalid email or password.");
                }
            }
        }

        public void LoginSuccessful(object sender, RoutedEventArgs e)
        {
            var quotationWindow = new QuotationWindow();
            quotationWindow.Show();
            this.Close();
        }
    }
}