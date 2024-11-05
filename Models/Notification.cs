using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace InterportCargoWPF.Models;

public class Notification
{
    public int Id { get; set; }
    public int CustomerId { get; set; }
    public string Message { get; set; }
    public DateTime DateCreated { get; set; }
    public bool IsRead { get; set; } = false; // Track whether the customer has read the notification

    // Navigation property to link the notification with a customer
    [ForeignKey("CustomerId")] public Customer Customer { get; set; }
}