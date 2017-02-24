using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Filters;

namespace Domain.Services
{
    public interface IMailFilterService
    {
        IEnumerable<IFilter> Filters { get; set; }

        Task StartFilterAsync();
    }
}
