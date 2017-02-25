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
        private readonly MailContext _context;

        public DbMailRepository()
        {
            _context = new MailContext();
        }

        public async Task<IEnumerable<EmailContent>> GetNotCheckedEmailsAsync()
        {
            //return new List<EmailContent>();
            return
                await _context.EmailContents.AsNoTracking().Where(s => s.Status == EmailStatus.NotChecked).ToListAsync();
        }

        public Task UpdateCheckEmailAsync(EmailContent email)
        {
            throw new NotImplementedException();
        }
    }
}
