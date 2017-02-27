using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.IRepository;
using Helpers;

namespace Services.Repositories
{
    public class FileMailRepository : IMailRepository
    {
        public string SourceDirectory { get; }
        public string DestinationDirectory { get; }


        public FileMailRepository(string src, string des)
        {
            // Init working directory here
            // Load the values from Application configuration file
            SourceDirectory = src;
            DestinationDirectory = des;
        }
        public async Task<IEnumerable<EmailContent>> GetNotCheckedEmailsAsync()
        {
            if (!Directory.Exists(SourceDirectory))
            {
                // SourceDirectory is provided by caller code. So the caller must handle the exception
                throw new Exception("The SourceDirectory is not exist");
            }

            // Get all files that the extension is .txt and filename is a Guid files
            var files = GetAllMailFiles();
            var emails = new List<EmailContent>();
            foreach (var file in files)
            {
                // Check file is free to read the content
                if (FileHelper.IsFileLocked(file))
                {
                    // The file is not ready to read content, so ignore it
                    continue;
                }

                try
                {
                    var content = await FileHelper.ReadContentAsync(file);
                    emails.Add(new EmailContent()
                    {
                        Content = content,
                        Status = EmailStatus.NotChecked,
                        EmailContentID = Guid.Parse(Path.GetFileNameWithoutExtension(file)),
                        MailSource = MailSource.FileSystem
                    });
                }
                catch (Exception ex)
                {
                    // Failed to load mail content, keep going
                    // throw;
                }

                
            }

            // Got list of emails
            return emails;
        }

        /// <summary>
        /// Save checked email
        /// 1) Create a copy with the status in name
        /// 2) Delete the email from source dir
        /// 
        /// + Failed to Create and delete
        /// + Success to Create Failed to delete
        /// + Success to Create and Delete
        /// 
        /// Need to cover all of the above in the production
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public async Task SaveCheckedEmailAsync(EmailContent email)
        {
            try
            {
                // Create a copy with validation status in name
                var des = Path.Combine(DestinationDirectory, $"{email.EmailContentID}_{(int)email.Status}.txt");
                using (var streamWriter = new StreamWriter(des))
                {
                    await streamWriter.WriteAsync(email.Content);
                }

                // Delete the old one
                File.Delete(Path.Combine(SourceDirectory, $"{email.EmailContentID}.txt"));
                // return email;
            }
            catch (Exception ex)
            {
                // return null;
                // throw;
            }
            
        }

        public async Task SaveCheckedEmailsAsync(List<EmailContent> emails)
        {
            var tasks = emails.Select(email => SaveCheckedEmailAsync(email)).ToList();
            await Task.WhenAll(tasks);
        }

        /// <summary>
        /// Get all mail files from source directory
        /// </summary>
        /// <returns></returns>
        private IEnumerable<string> GetAllMailFiles()
        {
            var files = Directory.GetFiles(SourceDirectory).Where(s => s.EndsWith(".txt") && IsFileNameAGuid(s));
            return files;
        }

        /// <summary>
        /// Check File name is a Guid value
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private bool IsFileNameAGuid(string path)
        {
            var fileName = Path.GetFileNameWithoutExtension(path);
            Guid id;
            bool isValid = Guid.TryParse(fileName, out id);
            return isValid;
        }
    }
}