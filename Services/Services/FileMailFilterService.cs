﻿using System;
using System.Collections.Concurrent;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;
using Domain.Entities;
using Domain.Filters;
using Domain.IRepository;
using Domain.Services;

namespace Services.Services
{
    public class FileMailFilterService : MailFilterService
    {
        private readonly ConcurrentQueue<EmailContent> _emailContentQueue;
        private readonly Timer _timer;
        private readonly IMailRepository _mailRepository;
        
        //public BindingList<EmailContent> EmailContentQueue { get; set; }
        //public IProgress<EmailContent> EnqueueProgress { get; set; }
        //public IProgress<EmailContent> DequeueProgress { get; set; }
        //public Collection<IFilter> Filters { get; set; }

        public FileMailFilterService(IMailRepository mailRepository)
        {
            Filters = new Collection<IFilter>();
            _mailRepository = mailRepository;
            _emailContentQueue = new ConcurrentQueue<EmailContent>();
            _timer = new Timer(2*1000);
            _timer.Elapsed += _timer_Elapsed;
            EmailContentQueue = new BindingList<EmailContent>();
            EnqueueProgress = new Progress<EmailContent>(s =>
            {
                EmailContentQueue.Add(s);
            });
        }

        private async void _timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            await Task.Run(DoFilterAsync);
        }

        

        public async Task StartFilterAsync()
        {
            await Task.Run(DoFilterAsync);
            _timer.Enabled = true;
            _timer.Start();
        }
        

        private async Task DoFilterAsync()
        {
            var emails = await _mailRepository.GetNotCheckedEmailsAsync();

            // Check to ignore the emails that are being in queue
            emails.ToList().ForEach(s =>
            {
                if (_emailContentQueue.All(t => t.EmailContentID != s.EmailContentID))
                {
                    _emailContentQueue.Enqueue(s);
                    EnqueueProgress.Report(s);
                }
            });
        }
    }
}
