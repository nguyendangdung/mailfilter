using System.Data.Entity;
using Domain.Entities;

namespace Services
{
    public class ValidationContext : DbContext
    {
        public DbSet<ValidationHistory> ValidationHistories { get; set; }
    }
}