using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using App.Properties;
using Domain;
using Domain.Entities;
using Domain.Filters;
using Domain.Services;
using Services;
using Helpers;
using Services.Filters;
using Services.Repositories;
using Services.Services;

namespace App
{
    public partial class Main : Form
    {
        // private bool _isClosing;
        private bool _isRunning;
        CancellationTokenSource _source;
        private readonly MailFilterService _fileMailFilterService;
        private readonly MailFilterService _dbMailFilterService;
        private readonly BindingList<EmailContent> _recentCheckedEmails;
        public Main()
        {
            InitializeComponent();
            _recentCheckedEmails = new BindingList<EmailContent>();

            srcTxt.Text = Settings.Default.src;
            desTxt.Text = Settings.Default.des;

            _fileMailFilterService =
                new MailFilterService(new FileMailRepository(Settings.Default.src, Settings.Default.des),
                    new ValidationHistoryRepository())
                {
                    OnEmailChecked = e => AddToRecentList(e),
                    Filters = new Collection<IFilter>()
                    {
                        new SendCustomerIDFilter(), new SensitiveInfoFilter()
                    }
                };

            _dbMailFilterService = new MailFilterService(new AdoMailRepository(), new AdoValidationHistoryRepository())
            {
                OnEmailChecked = e => AddToRecentList(e),
                Filters = new Collection<IFilter>()
                {
                    new SendCustomerIDFilter(),
                    new SensitiveInfoFilter()
                }
            };

            fileEmailContentBindingSource.DataSource = _fileMailFilterService.EmailContentQueue;
            dbEmailContentBindingSource.DataSource = _dbMailFilterService.EmailContentQueue;
            checkedEmailContentBindingSource.DataSource = _recentCheckedEmails;

            //_source.Token.Register(() =>
            //{
            //    _isRunning = false;
            //    startBtn.Enabled = !_isRunning;
            //    stopBtn.Enabled = _isRunning;
            //});
        }

        private async void startBtn_Click(object sender, EventArgs e)
        {
            if (CheckMailDirectories() && !_isRunning)
            {
                _source = new CancellationTokenSource();
                _source.Token.Register(() =>
                {
                    _isRunning = false;
                    startBtn.Enabled = !_isRunning;
                    stopBtn.Enabled = _isRunning;
                });
                _isRunning = true;
                startBtn.Enabled = !_isRunning;
                stopBtn.Enabled = _isRunning;
                var monitorTasks1 = _fileMailFilterService.MonitorAsync();
                var monitorTasks2 = _dbMailFilterService.MonitorAsync();
                await Task.WhenAll(monitorTasks1, monitorTasks2);

                var task1 = _fileMailFilterService.StartFilterAsync(_source.Token);
                var task2 = _dbMailFilterService.StartFilterAsync(_source.Token);

                // await Task.WhenAll(task1, task2);
                await Task.WhenAny(Task.WhenAll(task1, task2), _source.Token.AsTask());
            }
        }

        private void stopBtn_Click(object sender, EventArgs e)
        {
            stopBtn.Enabled = false;
            _fileMailFilterService.StopMonitor();
            _dbMailFilterService.StopMonitor();
            _source.Cancel();
        }

        private bool CheckMailDirectories()
        {
            return Directory.Exists(Settings.Default.src) && Directory.Exists(Settings.Default.des);
        }


        private void AddToRecentList(EmailContent email)
        {
            if (email == null)
            {
                throw new ArgumentNullException("email");
            }
            _recentCheckedEmails.Add(email);
            if (_recentCheckedEmails.Count > 100)
            {
                _recentCheckedEmails.RemoveAt(0);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // get search filter object
        }



        private SearchFilter GetSearchFilter()
        {
            var search = new SearchFilter()
            {
                FileNameOrId = fileNameOrIdTxt.Text,
                Content = emaiContentTxt.Text,
                From = @from.Value,
                To = to.Value,
                ViolatedContent = violatedContentTxt.Text,
                EmailStatuses = new List<EmailStatus>()
            };

            if (violatedChk.Checked)
            {
                search.EmailStatuses.Add(EmailStatus.Violated);
            }
            if (notViolatedChk.Checked)
            {
                search.EmailStatuses.Add(EmailStatus.NotViolated);
            }
            if (errorChk.Checked)
            {
                search.EmailStatuses.Add(EmailStatus.Error);
            }

            return search;
        }
    }
}
