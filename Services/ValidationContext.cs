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


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ValidationHistory>().Property(s => s.Content).IsRequired();
            modelBuilder.Entity<ValidationHistory>().Property(s => s.AsciiContent).IsRequired();


            base.OnModelCreating(modelBuilder);
        }
    }
}