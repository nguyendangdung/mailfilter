using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Services;
using Services.Services;

namespace App
{
    public class Controller : IMailFilterController
    {
        public ICollection<IMailFilterService> MailFilterServices { get; set; }

        public Controller()
        {
            MailFilterServices = new List<IMailFilterService>()
            {
                new FileMailFilterService()
            };
        }

        public IEnumerable<ValidationHistory> GetValidationHistories(int page, int size)
        {
            throw new NotImplementedException();
        }

        public Task StartFilterAsync()
        {
            throw new NotImplementedException();
        }
    }
}
