using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class ValidationHistory
    {
        public Guid ValidationHistoryID { get; set; }
        public string Content { get; set; }
        public EmailStatus Status { get; set; }
        public string Description { get; set; }
        public string FileName { get; set; }
        public Guid? EmailContentId { get; set; }
        public DateTime ValidationDTG { get; set; }
    }
}
