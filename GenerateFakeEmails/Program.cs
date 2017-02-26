using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using Domain.Entities;
using Services;

namespace GenerateFakeEmails
{
    class Program
    {
        static void Main(string[] args)
        {

            var source = @"C:\Users\dannguyen\Desktop\test";
            var ii = 0;
            using (var context = new MailContext())
            {
                while (true)
                {
                    Thread.Sleep(1000);
                    ii++;

                    
                    for (int i = 0; i < 50; i++)
                    {
                        var email = new EmailContent()
                        {
                            EmailContentID = Guid.NewGuid(),
                            Status = EmailStatus.NotChecked,
                            Content = "random content here"
                        };

                        File.WriteAllText(Path.Combine(source, email.EmailContentID + ".txt"), email.Content);
                    }

                    var emails = new List<EmailContent>();

                    for (int i = 0; i < 100; i++)
                    {
                        emails.Add(new EmailContent()
                        {
                            EmailContentID = Guid.NewGuid(),
                            Status = EmailStatus.NotChecked,
                            Content = "random content here"
                        });
                    }
                    context.EmailContents.AddRange(emails);
                    context.SaveChanges();
                    Console.WriteLine(ii);
                }
            }
        }
    }
}
