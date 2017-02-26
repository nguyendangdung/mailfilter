using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Domain.Entities;
using Domain.Services;
using Services;
using Services.Repositories;
using Services.Services;

namespace App
{
    public partial class Main : Form
    {
        private readonly List<IMailFilterService> _mailFilterServices;
        private readonly BindingList<EmailContent> _checkedEmails;
        public Main()
        {
            
            InitializeComponent();
            _checkedEmails = new BindingList<EmailContent>();

            var fileEmailService = new MailFilterService(new FileMailRepository(srcTxt.Text, desTxt.Text), new ValidationHistoryRepository())
            {
                OnEmailChecked = e =>
                {
                    _checkedEmails.Add(e);
                }
            };
            var dbEmailService = new MailFilterService(new DbMailRepository(), new ValidationHistoryRepository())
            {
                OnEmailChecked = e =>
                {
                    _checkedEmails.Add(e);
                }
            };

            _mailFilterServices = new List<IMailFilterService>()
            {
                fileEmailService, dbEmailService
            };

            fileEmailContentBindingSource.DataSource = fileEmailService.EmailContentQueue;
            dbEmailContentBindingSource.DataSource = dbEmailService.EmailContentQueue;
            checkedEmailContentBindingSource.DataSource = _checkedEmails;
        }

        private async void startBtn_Click(object sender, EventArgs e)
        {
            var monitorTasks = _mailFilterServices.Select(s => s.MonitorAsync());
            await Task.WhenAll(monitorTasks);

            var task1 = _mailFilterServices[0].StartFilterAsync();
            var task2 = _mailFilterServices[1].StartFilterAsync();

            await Task.WhenAll(task1, task2);
        }
    }
}
