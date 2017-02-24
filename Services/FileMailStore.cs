using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using Domain.Services;

namespace Services
{
    public class FileMailStore
    {
        private System.Timers.Timer _timer;
        private ICollection<IMailFilterService> _mailFilterServices;

        public FileMailStore()
        {
            _mailFilterServices = new List<IMailFilterService>();

            _timer = new Timer(1000*5);
            _timer.Elapsed += _timer_Elapsed;

        }


        public void AttachObserver(IMailFilterService mailFilterService)
        {
            if (mailFilterService == null)
            {
                throw new ArgumentNullException("mailFilterService");
            }

            _mailFilterServices.Add(mailFilterService);
        }

        public void DetachObserver(IMailFilterService mailFilterService)
        {
            if (mailFilterService == null)
            {
                throw new ArgumentNullException("mailFilterService");
            }

            _mailFilterServices.Remove(mailFilterService);
        }

        private void _timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            // Call actions on Observer objects
            throw new NotImplementedException();
        }
    }
}
