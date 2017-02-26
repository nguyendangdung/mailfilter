using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using Domain.Entities;
using GenerateFakeEmails.Properties;
using Services;

namespace GenerateFakeEmails
{
    class Program
    {
        private static Random random = new Random(1);
        private static string GetRandomContent()
        {

            var contents = new List<string>()
            {
                "this is valid content",
                "this contain customer id KH_123456 kh_askldfj kh_222222",
                "12345678             ksdfalksd 9999999999999999999999",
                "12121212 kh_111111 alskdjflas laskdfj asdkljf alskdjf ksdfj"
            };

            return contents[random.Next(0, 4)];

        }
        static void Main(string[] args)
        {

            var source = Settings.Default.src;
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
                            Content = GetRandomContent()
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
                            Content = GetRandomContent()
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
