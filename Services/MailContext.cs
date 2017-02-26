using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;

namespace Services
{
    public class MailContext : DbContext
    {
        public DbSet<EmailContent> EmailContents { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // modelBuilder.Entity<EmailContent>().Ignore(s => s.IsChecking);
            modelBuilder.Entity<EmailContent>().Ignore(s => s.MailSource);

            base.OnModelCreating(modelBuilder);
        }
    }
}
