using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;

namespace Domain.IRepository
{
    /// <summary>
    /// Handle EmailContent actions
    /// </summary>
    public interface IMailRepository
    {
        Task<IEnumerable<EmailContent>> GetNotCheckedEmailsAsync();
        Task UpdateCheckEmailAsync(EmailContent email);
        // Task Save
    }
}
