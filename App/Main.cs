using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using App.Properties;
using Domain.Entities;
using Domain.Services;
using Services;
using Services.Repositories;
using Services.Services;

namespace App
{
    public partial class Main : Form
    {
        private readonly MailFilterService _fileMailFilterService;
        private readonly MailFilterService _dbMailFilterService;
        private readonly BindingList<EmailContent> _checkedEmails;
        public Main()
        {
            InitializeComponent();
            _checkedEmails = new BindingList<EmailContent>();
            _fileMailFilterService =
                new MailFilterService(new FileMailRepository(Settings.Default.src, Settings.Default.des),
                    new ValidationHistoryRepository())
                {
                    OnEmailChecked = e =>
                    {
                        _checkedEmails.Add(e);
                    }
                };

            _dbMailFilterService = new MailFilterService(new DbMailRepository(), new ValidationHistoryRepository())
            {
                OnEmailChecked = e =>
                {
                    _checkedEmails.Add(e);
                }
            };

            fileEmailContentBindingSource.DataSource = _fileMailFilterService.EmailContentQueue;
            dbEmailContentBindingSource.DataSource = _dbMailFilterService.EmailContentQueue;
            checkedEmailContentBindingSource.DataSource = _checkedEmails;
        }

        private async void startBtn_Click(object sender, EventArgs e)
        {
            if (CheckMailDirectories())
            {
                var monitorTasks1 = _fileMailFilterService.MonitorAsync();
                var monitorTasks2 = _dbMailFilterService.MonitorAsync();
                await Task.WhenAll(monitorTasks1, monitorTasks2);

                var task1 = _fileMailFilterService.StartFilterAsync();
                var task2 = _dbMailFilterService.StartFilterAsync();

                await Task.WhenAll(task1, task2);
            }
        }

        private bool CheckMailDirectories()
        {
            return Directory.Exists(Settings.Default.src) && Directory.Exists(Settings.Default.des);
        }
    }
}
