using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using Domain.Entities;
using Domain.Filters;
using Domain.IRepository;
using Domain.Services;

namespace Services.Services
{
    public class MailFilterService : IMailFilterService
    {
        private readonly ConcurrentQueue<EmailContent> _emailContentQueue;
        private readonly Timer _timer;
        private readonly IMailRepository _mailRepository;
        private readonly IValidationHistoryRepository _validationHistoryRepository;

        public MailFilterService(IMailRepository mailRepository, IValidationHistoryRepository validationHistoryRepository)
        {
            _emailContentQueue = new ConcurrentQueue<EmailContent>();
            _mailRepository = mailRepository;
            _validationHistoryRepository = validationHistoryRepository;
            EmailContentQueue = new BindingList<EmailContent>();
            Filters = new Collection<IFilter>();

            _timer = new Timer(2 * 1000);
            _timer.Elapsed += _timer_Elapsed;

            EnqueueProgress = new Progress<EmailContent>(s =>
            {
                EmailContentQueue.Add(s);
            });
            DequeueProgress = new Progress<EmailContent>(s =>
            {
                EmailContentQueue.Remove(s);
                OnEmailChecked?.Invoke(s);
            });
        }

        public Action<EmailContent> OnEmailChecked;

        public IProgress<EmailContent> EnqueueProgress { get; set; }
        public IProgress<EmailContent> DequeueProgress { get; set; }
        public BindingList<EmailContent> EmailContentQueue { get; set; }
        public Collection<IFilter> Filters { get; set; }


        public void AddFilter(IFilter filter)
        {
            if (filter == null)
            {
                throw new ArgumentNullException("filter");
            }
            Filters.Add(filter);
        }
        public void RemoveFilter(IFilter filter)
        {
            if (filter == null)
            {
                throw new ArgumentNullException("filter");
            }
            Filters.Remove(filter);
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

            // Try dequeue a email from the queue
            while (!_emailContentQueue.IsEmpty)
            {
                EmailContent email;
                var result = _emailContentQueue.TryDequeue(out email);
                if (result && email != null)
                {
                    // Get FilterResults from filters
                    var filterResults = Filters.Select(s => s.CheckMail(email)).ToList();
                    if (filterResults.Any(s => s.Status == EmailStatus.Violated))
                    {
                        email.Status = EmailStatus.Violated;
                        
                    }

                    // Save processed EmailContent
                    await _mailRepository.UpdateCheckEmailAsync(email);

                    // Generate ValidationHistory Object
                    var validationHistory = GetValidationHistory(email, filterResults);
                    // Save to the storage
                    await _validationHistoryRepository.AddAsync(validationHistory);
                    DequeueProgress.Report(email);
                }
            }
        }

        private ValidationHistory GetValidationHistory(EmailContent email, List<FilterResult> filterResults)
        {
            if (filterResults == null)
            {
                throw new ArgumentNullException("filterResults");
            }
            var message = string.Empty;
            filterResults.Where(s => s.Status == EmailStatus.Violated).ToList().ForEach(s =>
            {
                message += s.Message + ", ";
            });

            if (!string.IsNullOrWhiteSpace(message))
            {
                message = message.Trim();
                message = message.Substring(0, message.Length - 1);
            }

            return new ValidationHistory()
            {
                Status = email.Status,
                Content = email.Content,
                EmailContentId = email.MailSource == MailSource.Db ? email.EmailContentID : (Guid?)null,
                ValidationDTG = DateTime.Now,
                ValidationHistoryID = Guid.NewGuid(),
                Description = message,
                FileName = email.MailSource == MailSource.FileSystem ? "" : null
            };
        }

        private async void _timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            await Task.Run(DoFilterAsync);
        }
    }
}
