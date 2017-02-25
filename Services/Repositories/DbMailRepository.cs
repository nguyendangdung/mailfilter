using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.IRepository;

namespace Services.Repositories
{
    public class DbMailRepository : IMailRepository
    {
        public Task<IEnumerable<EmailContent>> GetNotCheckedEmailsAsync()
        {
            throw new NotImplementedException();
        }

        public Task UpdateCheckEmailAsync(EmailContent email)
        {
            throw new NotImplementedException();
        }
    }
}
