using InterportCargoWPF.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace InterportCargoWPF
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
        }
        private void LoginButton_Click(object sender, RoutedEventArgs e)
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
                    // Proceed to quotation or next functionality
                }
                else
                {
                    MessageBox.Show("Invalid email or password.");
                }
            }
        }
    }
}