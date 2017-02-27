using System.Data.Entity;
using Domain.Entities;
using Serilog;

namespace Services
{
    public class ValidationContext : DbContext
    {
        public DbSet<ValidationHistory> ValidationHistories { get; set; }

        public ValidationContext()
        {
            Database.Log = Log.Information;
        }
    }
}