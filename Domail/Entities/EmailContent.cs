using System;

namespace Domain.Entities
{
    public class EmailContent
    {
        public Guid EmailContentID { get; set; }
        public string Content { get; set; }
        public EmailStatus Status { get; set; }
        public bool IsChecking { get; set; }
    }
}