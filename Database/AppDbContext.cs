using InterportCargoWPF.Models;
using Microsoft.EntityFrameworkCore;

namespace InterportCargoWPF.Database;

public class AppDbContext : DbContext
{
    public DbSet<Customer?> Customers { get; set; }
    public DbSet<Quotation> Quotations { get; set; }
    public DbSet<Employee> Employees { get; set; }
    public DbSet<Notification> Notifications { get; set; }
    public DbSet<RateSchedule> RateSchedules { get; set; }


    // This is where you configure SQLite
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // This sets the SQLite database file path
        optionsBuilder.UseSqlite(
            @"Data Source=C:\Users\61414\RiderProjects\InterportCargoWPF\Database\interportcargo.db");
    }
}