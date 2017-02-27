using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.IRepository;

namespace Services.Repositories
{
    public class MailRepository : IMailRepository
    {

        private readonly string _connectonString = ConfigurationManager.ConnectionStrings["MailContext"].ConnectionString;

        public async Task<IEnumerable<EmailContent>> GetNotCheckedEmailsAsync()
        {
            var result = new List<EmailContent>();
            using (var con = new SqlConnection(_connectonString))
            {
                await con.OpenAsync();
                var sql =
                "SELECT [EmailContentID], [Content], [Status] " +
                "FROM[dbo].[EmailContents] AS[Extent1] " +
                $"WHERE {(int)EmailStatus.NotChecked} = [Status]";
                using(var cmd = new SqlCommand(sql, con))
                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        result.Add(new EmailContent()
                        {
                            EmailContentID = (Guid)reader["EmailContentID"],
                            Status = (EmailStatus)reader["Status"],
                            Content = reader["Content"].ToString(),
                            MailSource = MailSource.Db
                        });
                    }
                    
                }
                return result;
            }
            
        }

        public Task SaveCheckedEmailAsync(EmailContent email)
        {
            throw new NotImplementedException();
        }

        public async Task SaveCheckedEmailsAsync(List<EmailContent> emails)
        {
            if (emails == null)
            {
                throw new ArgumentNullException(nameof(emails));
            }

            if (emails.Any())
            {
                using (var con = new SqlConnection(_connectonString))
                {
                    await con.OpenAsync();
                    string sql = "";

                    emails.ForEach(s =>
                    {
                        var str = $"UPDATE EmailContents SET Status = {(int)s.Status} WHERE EmailContentID = '{s.EmailContentID}';";
                        sql += str;
                    });

                    using (var cmd = new SqlCommand(sql, con))
                    {
                        await cmd.ExecuteNonQueryAsync();
                    }
                }
            }
            
        }
    }
}
