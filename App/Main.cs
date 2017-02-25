using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Domain.Services;
using Services.Repositories;
using Services.Services;

namespace App
{
    public partial class Main : Form
    {
        private readonly List<IMailFilterService> _mailFilterServices;
        // private readonly IProgress<string> _fileMailsProcess = new Progress<string>(s => { });
        private IProgress<string> _dbMailsProcess = new Progress<string>(s => { });
        public Main()
        {
            
            InitializeComponent();
            var fileEmailService = new FileMailFilterService(new FileMailRepository(srcTxt.Text, desTxt.Text));
            _mailFilterServices = new List<IMailFilterService>()
            {
                fileEmailService
            };
            fileEmailContentBindingSource.DataSource = fileEmailService.EmailContentQueue;
        }

        private async void startBtn_Click(object sender, EventArgs e)
        {
            var tasks = _mailFilterServices.Select(s => s.StartFilterAsync());
            await Task.WhenAll(tasks);
        }
    }
}
