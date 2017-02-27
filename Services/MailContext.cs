using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using Serilog;

namespace Services
{
    public class MailContext : DbContext
    {
        public DbSet<EmailContent> EmailContents { get; set; }

        public MailContext()
        {
            Database.Log = Log.Information;
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // modelBuilder.Entity<EmailContent>().Ignore(s => s.IsChecking);
            modelBuilder.Entity<EmailContent>().Ignore(s => s.MailSource);
            modelBuilder.Entity<EmailContent>().Property(s => s.Content).IsRequired();

            base.OnModelCreating(modelBuilder);
        }
    }
}
