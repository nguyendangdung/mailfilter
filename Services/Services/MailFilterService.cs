using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
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

            _timer = new Timer(1000);
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
            await DoFilterAsync();
        }

        public async Task MonitorAsync()
        {
            await Task.Run(DoMonitorAsync);
        }

        /// <summary>
        /// This is excuted in separate thread
        /// </summary>
        /// <returns></returns>
        private async Task DoMonitorAsync()
        {
            // Still there are too many emails in the queue => Ignore
            if (_emailContentQueue.Count < 1000)
            {
                _timer.Stop();
                var emails = await _mailRepository.GetNotCheckedEmailsAsync();
                emails.ToList().ForEach(s =>
                {
                    if (_emailContentQueue.All(t => t.EmailContentID != s.EmailContentID))
                    {
                        _emailContentQueue.Enqueue(s);
                        EnqueueProgress.Report(s);
                    }
                });
                _timer.Start();
            }
        }


        private async Task DoFilterAsync()
        {
            var emails = new List<EmailContent>();
            var validationHistories = new List<ValidationHistory>();
            while (true)
            {
                if (!_emailContentQueue.IsEmpty)
                {
                    EmailContent email;
                    var result = _emailContentQueue.TryDequeue(out email);
                    if (result && email != null)
                    {
                        // Get FilterResults from filters
                        var filterResults = Filters.Select(s => s.CheckMail(email)).ToList();
                        email.Status = filterResults.Any(s => s.Status == EmailStatus.Violated)
                            ? EmailStatus.Violated
                            : EmailStatus.NotViolated;

                        // Add email to the template list
                        emails.Add(email);

                        // Generate ValidationHistory Object
                        var validationHistory = GetValidationHistory(email, filterResults);

                        // Add ValidationHistory to the template list
                        validationHistories.Add(validationHistory);

                        // Update working list
                        DequeueProgress.Report(email);

                        if (emails.Count >= 100)
                        {
                            await _mailRepository.SaveCheckedEmailsAsync(emails);
                            await _validationHistoryRepository.AddRangeAsync(validationHistories);

                            emails.Clear();
                            validationHistories.Clear();
                        }
                    }
                }
                else
                {
                    if (emails.Any())
                    {
                        await _mailRepository.SaveCheckedEmailsAsync(emails);
                        await _validationHistoryRepository.AddRangeAsync(validationHistories);

                        emails.Clear();
                        validationHistories.Clear();
                    }
                    await Task.Delay(1000);
                }
            }
        }

        private ValidationHistory GetValidationHistory(EmailContent email, List<FilterResult> filterResults)
        {
            if (filterResults == null)
            {
                throw new ArgumentNullException(nameof(filterResults));
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
            // Repeat the action
            await DoMonitorAsync();
        }
    }
}
