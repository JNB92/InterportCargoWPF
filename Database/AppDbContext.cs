using InterportCargoWPF.Models;
using Microsoft.EntityFrameworkCore;

namespace InterportCargoWPF.Database;

/// <summary>
///     Represents the database context for the application, providing access to entities such as customers, quotations,
///     employees, notifications, and rate schedules.
///     Configures the database connection and manages data access.
/// </summary>
public class AppDbContext : DbContext
{
    /// <summary>
    ///     Gets or sets the <see cref="DbSet{Customer}" /> for accessing customer data.
    /// </summary>
    public DbSet<Customer> Customers { get; set; }

    /// <summary>
    ///     Gets or sets the <see cref="DbSet{Quotation}" /> for accessing quotation data.
    /// </summary>
    public DbSet<Quotation> Quotations { get; set; }

    /// <summary>
    ///     Gets or sets the <see cref="DbSet{Employee}" /> for accessing employee data.
    /// </summary>
    public DbSet<Employee> Employees { get; set; }

    /// <summary>
    ///     Gets or sets the <see cref="DbSet{Notification}" /> for accessing notification data.
    /// </summary>
    public DbSet<Notification> Notifications { get; set; }

    /// <summary>
    ///     Gets or sets the <see cref="DbSet{RateSchedule}" /> for accessing rate schedule data.
    /// </summary>
    public DbSet<RateSchedule> RateSchedules { get; set; }

    /// <summary>
    ///     Configures the database to use SQLite, setting the file path for the database file.
    /// </summary>
    /// <param name="optionsBuilder">The options builder used to configure the database connection.</param>
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite(
            @"Data Source=C:\Users\61414\RiderProjects\InterportCargoWPF\Database\interportcargo.db");
    }
}