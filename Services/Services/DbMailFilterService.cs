using System;
using System.Collections.Concurrent;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Timers;
using Domain.Entities;
using Domain.Filters;
using Domain.IRepository;
using Domain.Services;

namespace Services.Services
{
    public class DbMailFilterService : IMailFilterService
    {

        private readonly ConcurrentQueue<EmailContent> _emailContentQueue;
        private readonly Timer _timer;
        private readonly IMailRepository _mailRepository;

        public DbMailFilterService(IMailRepository mailRepository)
        {
            _mailRepository = mailRepository;
            Filters = new Collection<IFilter>();
            _emailContentQueue = new ConcurrentQueue<EmailContent>();
            _timer = new Timer(2 * 1000);
            _timer.Elapsed += _timer_Elapsed;
            EmailContentQueue = new BindingList<EmailContent>();
            EnqueueProgress = new Progress<EmailContent>(s =>
            {
                EmailContentQueue.Add(s);
            });
        }

        private void _timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            await Task.Run(DoFilterAsync);
        }

        public IProgress<EmailContent> EnqueueProgress { get; set; }
        public IProgress<EmailContent> DequeueProgress { get; set; }
        public BindingList<EmailContent> EmailContentQueue { get; set; }

        public Collection<IFilter> Filters { get; set; } = new Collection<IFilter>();
        public void AddFilter(IFilter filter)
        {
            throw new NotImplementedException();
        }

        public void RemoveFilter(IFilter filter)
        {
            throw new NotImplementedException();
        }

        public Task StartFilterAsync()
        {
            throw new NotImplementedException();
        }
    }
}
