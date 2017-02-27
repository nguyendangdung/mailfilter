using System;

namespace Domain.Entities
{
    public enum MailSource
    {
        FileSystem, Db
    }
    public class EmailContent
    {
        public Guid EmailContentID { get; set; }
        public string Content { get; set; }
        public EmailStatus Status { get; set; }
        public MailSource MailSource { get; set; }
    }
}