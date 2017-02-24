using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Filters;
using Domain.Services;

namespace Services.Services
{
    public class FileMailFilterService : IMailFilterService
    {
        public IEnumerable<IFilter> Filters { get; set; }

        public async Task StartFilterAsync()
        {
            throw new NotImplementedException();
        }
    }
}
