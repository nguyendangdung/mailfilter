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
        Task<EmailContent> UpdateCheckEmailAsync(EmailContent email);

        /// <summary>
        /// Save Checked emails
        /// </summary>
        /// <param name="emails"></param>
        /// <returns></returns>
        Task<List<EmailContent>> UpdateCheckEmailsAsync(List<EmailContent> emails);
        // Task Save
    }
}
