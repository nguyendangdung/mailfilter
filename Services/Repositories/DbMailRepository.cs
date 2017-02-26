using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.IRepository;

namespace Services.Repositories
{
    public class DbMailRepository : IMailRepository
    {
        public async Task<IEnumerable<EmailContent>> GetNotCheckedEmailsAsync()
        {
            using (var context = new MailContext())
            {
                var items = await
                    context.EmailContents.AsNoTracking()
                        .Where(s => s.Status == EmailStatus.NotChecked).ToListAsync();
                items.ForEach(s => s.MailSource = MailSource.Db);
                return items;
            }
            
        }

        public async Task<EmailContent> UpdateCheckEmailAsync(EmailContent email)
        {
            try
            {
                using (var context = new MailContext())
                {
                    context.EmailContents.Attach(email);
                    context.Entry(email).State = EntityState.Modified;
                    await context.SaveChangesAsync();
                    return email;
                }
            }
            catch (Exception)
            {
                return null;
            }
            
        }

        public async Task<List<EmailContent>> UpdateCheckEmailsAsync(List<EmailContent> emails)
        {
            try
            {
                using (var context = new MailContext())
                {
                    emails.ForEach(s =>
                    {
                        context.EmailContents.Attach(s);
                        context.Entry(s).State = EntityState.Modified;

                    });
                    await context.SaveChangesAsync();
                    return emails;
                }
            }
            catch (Exception)
            {
                return new List<EmailContent>();
            }
            
        }
    }
}
