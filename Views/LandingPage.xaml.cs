using System.Windows;
using System.Windows.Controls;
using InterportCargoWPF.Database;
using System.Linq;

namespace InterportCargoWPF.Views
{
    /// <summary>
    /// Represents the landing page for customers, allowing navigation to various functionalities.
    /// </summary>
    public partial class LandingPage : Page
    {
        private int _customerId;

        /// <summary>
        /// Initializes a new instance of the <see cref="LandingPage" /> class.
        /// </summary>
        public LandingPage()
        {
            InitializeComponent();
            _customerId = SessionManager.LoggedInCustomerId; // Retrieve logged-in customer ID
            LoadNotifications(); // Load notifications when the page is initialized
        }

        /// <summary>
        /// Event handler for the "View My Quotations" button click.
        /// Navigates the customer to the <see cref="CustomerQuotationsPage" />.
        /// </summary>
        /// <param name="sender">The button that was clicked.</param>
        /// <param name="e">The event data.</param>
        private void ViewMyQuotations_Click(object sender, RoutedEventArgs e)
        {
            if (_customerId <= 0)
            {
                MessageBox.Show("Customer ID is not valid. Please log in again.");
                return;
            }

            // Navigate to CustomerQuotationsPage within the ContentFrame
            ContentFrame.Navigate(new CustomerQuotationsPage(_customerId));
        }

        /// <summary>
        /// Loads and displays unread notifications for the logged-in customer.
        /// Marks notifications as read after displaying them.
        /// </summary>
        private void LoadNotifications()
        {
            using (var context = new AppDbContext())
            {
                var notifications = context.Notifications
                    .Where(n => n.CustomerId == _customerId && !n.IsRead)
                    .OrderByDescending(n => n.DateCreated)
                    .ToList();

                NotificationsPanel.Children.Clear(); // Clear previous notifications

                if (notifications.Any())
                {
                    foreach (var notification in notifications)
                    {
                        AddNotification(notification.Message);
                        notification.IsRead = true; // Mark notification as read
                    }

                    context.SaveChanges(); // Save changes to the database
                }
            }
        }

        /// <summary>
        /// Adds a notification to the NotificationsPanel.
        /// </summary>
        /// <param name="message">The notification message to add.</param>
        private void AddNotification(string message)
        {
            TextBlock notificationText = new TextBlock
            {
                Text = message,
                Margin = new Thickness(0, 0, 20, 0), // Margin for spacing between notifications
                VerticalAlignment = VerticalAlignment.Center
            };

            NotificationsPanel.Children.Add(notificationText);
        }

        /// <summary>
        /// Event handler for the "Go To Quotation" button click.
        /// Navigates the customer to the <see cref="QuotationPage" />.
        /// </summary>
        /// <param name="sender">The button that was clicked.</param>
        /// <param name="e">The event data.</param>
        private void GoToQuotation_Click(object sender, RoutedEventArgs e)
        {
            // Navigate to QuotationPage within the ContentFrame
            ContentFrame.Navigate(new QuotationPage(_customerId));
        }

        /// <summary>
        /// Event handler for the "Outturn" button click.
        /// Navigates to the <see cref="OutturnPage" />.
        /// </summary>
        private void OutturnButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new OutturnPage());
        }

        /// <summary>
        /// Event handler for the "Booking" button click.
        /// Navigates to the <see cref="BookingPage" />.
        /// </summary>
        private void BookingButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new BookingPage());
        }
    }
}
