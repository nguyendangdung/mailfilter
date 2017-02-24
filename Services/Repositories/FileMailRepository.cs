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
        public string DestinationDirectory { get; private set; }


        public FileMailRepository()
        {
            // Init working directory here
            // Load the values from Application configuration file
            SourceDirectory = "";
            DestinationDirectory = "";
        }
        public async Task<IEnumerable<EmailContent>> GetAllAsyc()
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
                    continue;
                }

                emails.Add(new EmailContent()
                {
                    Content = FileHelper.ReadContent(file),
                    Status = EmailStatus.NotChecked,
                    EmailContentID = Guid.Parse(Path.GetFileNameWithoutExtension(file))
                });

            }

            return emails;
        }

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