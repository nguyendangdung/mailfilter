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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="email"></param>
        /// <returns>Return the email if success</returns>
        Task SaveCheckedEmailAsync(EmailContent email);

        /// <summary>
        /// Save Checked emails
        /// </summary>
        /// <param name="emails"></param>
        /// <returns></returns>
        Task SaveCheckedEmailsAsync(List<EmailContent> emails);
        // Task Save
    }
}
