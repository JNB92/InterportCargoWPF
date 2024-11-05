using System.Windows;
using InterportCargoWPF.Database;

namespace InterportCargoWPF.Views;

public partial class LandingPage 
{
    private int _customerId;

    public LandingPage()
    {
        InitializeComponent();
    }

    private void ViewMyQuotations_Click(object sender, RoutedEventArgs e)
    {
        var loggedInCustomerId = SessionManager.LoggedInCustomerId;

        if (loggedInCustomerId <= 0)
        {
            MessageBox.Show("Customer ID is not valid. Please log in again.");
            return;
        }

        // Navigate to CustomerQuotationsPage within the ContentFrame
        ContentFrame.Navigate(new CustomerQuotationsPage(loggedInCustomerId));
    }
    private void LoadNotifications()
    {
        using (var context = new AppDbContext())
        {
            var notifications = context.Notifications
                .Where(n => n.CustomerId == _customerId && !n.IsRead)
                .OrderByDescending(n => n.DateCreated)
                .ToList();

            if (notifications.Any())
            {
                var notificationMessage = string.Join("\n", notifications.Select(n => n.Message));
                MessageBox.Show(notificationMessage, "Notifications");

                foreach (var notification in notifications) notification.IsRead = true;

                context.SaveChanges();
            }
        }
    }

    private void GoToQuotation_Click(object sender, RoutedEventArgs e)
    {
        var loggedInCustomerId = SessionManager.LoggedInCustomerId;

        // Navigate to QuotationPage within the ContentFrame
        ContentFrame.Navigate(new QuotationPage(loggedInCustomerId));
    }

    private void OutturnButton_Click(object sender, RoutedEventArgs e)
    {
        NavigationService.Navigate(new OutturnPage());
    }

    private void BookingButton_Click(object sender, RoutedEventArgs e)
    {
        NavigationService.Navigate(new BookingPage());
    }
}