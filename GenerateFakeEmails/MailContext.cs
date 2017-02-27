using System.Data.Entity;
using Domain.Entities;

namespace GenerateFakeEmails
{
    public class MailContext : DbContext
    {
        public DbSet<EmailContent> EmailContents { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // modelBuilder.Entity<EmailContent>().Ignore(s => s.IsChecking);
            modelBuilder.Entity<EmailContent>().Ignore(s => s.MailSource);
            modelBuilder.Entity<EmailContent>().Property(s => s.Content).IsRequired();

            base.OnModelCreating(modelBuilder);
        }
    }
}
