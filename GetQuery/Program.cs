using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Services.Repositories;

namespace GetQuery
{
    class Program
    {
        static void Main(string[] args)
        {
            var mailrepo = new DbMailRepository();
            var all = mailrepo.GetNotCheckedEmailsAsync().Result;
        }
    }
}
