using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Domain.Entities;
using Services;

namespace GenerateFakeEmails
{
    class Program
    {
        static void Main(string[] args)
        {
            var source = @"C:\Users\dannguyen\Desktop\test";
            var i = 0;
            using (var context = new MailContext())
            {
                while (i < 1000)
                {
                    Thread.Sleep(1000);
                    i++;
                    var email = new EmailContent()
                    {
                        EmailContentID = Guid.NewGuid(),
                        Status = EmailStatus.NotChecked,
                        Content = "random content here"
                    };

                    File.WriteAllText(Path.Combine(source, email.EmailContentID + ".txt"), email.Content);

                    email.EmailContentID = Guid.NewGuid();
                    context.EmailContents.Add(email);
                    context.SaveChanges();
                    Console.WriteLine(email.EmailContentID);
                }
            }
            


            Console.WriteLine("Done");
        }
    }
}
