using System.Windows;
using System.Windows.Controls;
using InterportCargoWPF.Database;

namespace InterportCargoWPF.Views;

/// <summary>
///     Represents the landing page for customers, allowing navigation to various functionalities.
/// </summary>
public partial class LandingPage : Page
{
    private int _customerId;

    /// <summary>
    ///     Initializes a new instance of the <see cref="LandingPage" /> class.
    /// </summary>
    public LandingPage()
    {
        InitializeComponent();
    }

    /// <summary>
    ///     Event handler for the "View My Quotations" button click.
    ///     Navigates the customer to the <see cref="CustomerQuotationsPage" />.
    /// </summary>
    /// <param name="sender">The button that was clicked.</param>
    /// <param name="e">The event data.</param>
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

    /// <summary>
    ///     Loads and displays unread notifications for the logged-in customer.
    ///     Marks notifications as read after displaying them.
    /// </summary>
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

                // Mark notifications as read
                foreach (var notification in notifications)
                    notification.IsRead = true;

                context.SaveChanges();
            }
        }
    }

    /// <summary>
    ///     Event handler for the "Go To Quotation" button click.
    ///     Navigates the customer to the <see cref="QuotationPage" />.
    /// </summary>
    /// <param name="sender">The button that was clicked.</param>
    /// <param name="e">The event data.</param>
    private void GoToQuotation_Click(object sender, RoutedEventArgs e)
    {
        var loggedInCustomerId = SessionManager.LoggedInCustomerId;

        // Navigate to QuotationPage within the ContentFrame
        ContentFrame.Navigate(new QuotationPage(loggedInCustomerId));
    }

    /// <summary>
    ///     Event handler for the "Outturn" button click.
    ///     Navigates to the <see cref="OutturnPage" />.
    /// </summary>
    private void OutturnButton_Click(object sender, RoutedEventArgs e)
    {
        NavigationService.Navigate(new OutturnPage());
    }

    /// <summary>
    ///     Event handler for the "Booking" button click.
    ///     Navigates to the <see cref="BookingPage" />.
    /// </summary>
    private void BookingButton_Click(object sender, RoutedEventArgs e)
    {
        NavigationService.Navigate(new BookingPage());
    }
}