using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace InterportCargoWPF.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Quotation> Quotations { get; set; }

        // This is where you configure SQLite
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // This sets the SQLite database file path
            optionsBuilder.UseSqlite("Data Source=interportcargo.db");
        }
    }
}
