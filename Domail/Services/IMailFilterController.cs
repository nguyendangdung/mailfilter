using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;

namespace Domain.Services
{
    public interface IMailFilterController
    {
        ICollection<IMailFilterService> MailFilterServices { get; set; }


        IEnumerable<ValidationHistory> GetValidationHistories(int page, int size);

        Task StartFilterAsync();
    }
}
