using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.IRepository;

namespace Services.Repositories
{
    public class DbMailRepository : IMailRepository
    {
        public Task<IEnumerable<EmailContent>> GetAllAsyc()
        {
            throw new NotImplementedException();
        }
    }
}
