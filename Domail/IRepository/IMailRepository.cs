using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;

namespace Domain.IRepository
{
    public interface IMailRepository
    {
        Task<IEnumerable<EmailContent>> GetAllAsyc();
    }
}
