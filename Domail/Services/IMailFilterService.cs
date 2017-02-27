using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Filters;

namespace Domain.Services
{
    public interface IMailFilterService
    {
        IProgress<EmailContent> EnqueueProgress { get; set; }
        IProgress<EmailContent> DequeueProgress { get; set; }
        BindingList<EmailContent> EmailContentQueue { get; set; }

        Collection<IFilter> Filters { get; set; }

        void AddFilter(IFilter filter);

        void RemoveFilter(IFilter filter);

        Task StartFilterAsync(CancellationToken cancellationToken);
        Task MonitorAsync();
    }
}
