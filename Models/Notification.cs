using System.ComponentModel.DataAnnotations.Schema;

namespace InterportCargoWPF.Models;

/// <summary>
///     Represents a notification sent to a customer, containing information about the message, date, and read status.
/// </summary>
public class Notification
{
    /// <summary>
    ///     Gets or sets the unique identifier for the notification.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    ///     Gets or sets the ID of the customer associated with the notification.
    /// </summary>
    public int CustomerId { get; set; }

    /// <summary>
    ///     Gets or sets the message content of the notification.
    /// </summary>
    public string Message { get; set; }

    /// <summary>
    ///     Gets or sets the date and time when the notification was created.
    /// </summary>
    public DateTime DateCreated { get; set; }

    /// <summary>
    ///     Gets or sets a value indicating whether the notification has been read by the customer.
    ///     Defaults to <c>false</c>.
    /// </summary>
    public bool IsRead { get; set; } = false;

    /// <summary>
    ///     Gets or sets the customer associated with the notification.
    /// </summary>
    [ForeignKey("CustomerId")]
    public Customer Customer { get; set; }
}