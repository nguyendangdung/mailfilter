using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain;
using Domain.Entities;
using Domain.IRepository;
using Helpers;

namespace Services.Repositories
{
    public class AdoValidationHistoryRepository : IValidationHistoryRepository
    {
        private readonly string _connectionString = ConfigurationManager.ConnectionStrings["ValidationContext"].ConnectionString;
        public async Task<IEnumerable<ValidationHistory>> GetAllAsync(SearchFilter filter, int page = 1, int size = 20)
        {
            var result = new List<ValidationHistory>();
            using (var con = new SqlConnection(_connectionString))
            {
                await con.OpenAsync();
                using (var cmd = new SqlCommand("Search", con) {CommandType = CommandType.StoredProcedure})
                {
                    cmd.Parameters.AddWithValue("@fileNameOrId",
                        string.IsNullOrWhiteSpace(filter?.FileNameOrId) ? DBNull.Value : (object)filter.FileNameOrId);
                    cmd.Parameters.AddWithValue("@emailContent",
                        string.IsNullOrWhiteSpace(filter?.Content) ? DBNull.Value : (object)UnicodeStrings.LatinToAscii(filter.Content));
                    cmd.Parameters.AddWithValue("@violatedContent",
                        string.IsNullOrWhiteSpace(filter?.ViolatedContent) ? DBNull.Value : (object)filter.ViolatedContent);


                    cmd.Parameters.AddWithValue("@from", (object)filter?.From ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@to", (object)filter?.To ?? DBNull.Value);

                    cmd.Parameters.AddWithValue("@violated", filter?.Violated == true ? (object)(int)EmailStatus.Violated : DBNull.Value);
                    cmd.Parameters.AddWithValue("@notViolated", filter?.NotViolated == true ? (object)(int)EmailStatus.NotViolated : DBNull.Value);
                    cmd.Parameters.AddWithValue("@error", filter?.Error == true ? (object)(int)EmailStatus.Error : DBNull.Value);

                    //cmd.Parameters.AddWithValue("@violated", DBNull.Value);
                    //cmd.Parameters.AddWithValue("@notViolated", DBNull.Value);
                    //cmd.Parameters.AddWithValue("@error", DBNull.Value);

                    var sql = cmd.CommandAsSql();

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (reader.Read())
                        {
                            try
                            {
                                result.Add(new ValidationHistory()
                                {
                                    Status = (EmailStatus)(int)reader["Status"],
                                    Content = reader["Content"].ToString(),
                                    ValidationHistoryID = (Guid)reader["ValidationHistoryID"],
                                    ValidationDTG = (DateTime)reader["ValidationDTG"],
                                    EmailContentId = reader["EmailContentId"] is DBNull ? null : (Guid?)reader["EmailContentId"],
                                    Description = reader["Description"]?.ToString(),
                                    FileName = reader["FileName"]?.ToString()
                                });
                            }
                            catch (Exception ex)
                            {
                                
                                throw;
                            }
                            
                        }

                        return result;
                    }
                }
            }
        }

        public Task AddAsync(ValidationHistory item)
        {
            throw new NotImplementedException();
        }

        public async Task AddRangeAsync(List<ValidationHistory> items)
        {
            if (items == null)
            {
                throw new ArgumentNullException(nameof(items));
            }
            if (items.Any())
            {
                using (var con = new SqlConnection(_connectionString))
                {
                    await con.OpenAsync();
                    var sql =
                        "INSERT [dbo].[ValidationHistories]([ValidationHistoryID], [Content], [Status], [Description], [FileName], [EmailContentId], [ValidationDTG], [AsciiContent]) " +
                        "VALUES (@0, @1, @2, @3, @4, @5, @6, @7);";
                    using (var cmd = new SqlCommand(sql, con))
                    {
                        foreach (var s in items)
                        {
                            cmd.Parameters.Clear();
                            cmd.Parameters.Add(new SqlParameter("@0", SqlDbType.UniqueIdentifier)
                            {
                                Value = s.ValidationHistoryID
                            });
                            cmd.Parameters.Add(new SqlParameter("@1", SqlDbType.NVarChar)
                            {
                                Value = s.Content.GetDbValue()
                            });
                            cmd.Parameters.Add(new SqlParameter("@2", SqlDbType.Int)
                            {
                                Value = (int)s.Status.GetDbValue()
                            });
                            cmd.Parameters.Add(new SqlParameter("@3", SqlDbType.NVarChar)
                            {
                                Value = s.Description.GetDbValue()
                            });
                            cmd.Parameters.Add(new SqlParameter("@4", SqlDbType.NVarChar)
                            {
                                Value = s.FileName.GetDbValue()
                            });
                            cmd.Parameters.Add(new SqlParameter("@5", SqlDbType.UniqueIdentifier)
                            {
                                Value = s.EmailContentId.GetDbValue()
                            });
                            cmd.Parameters.Add(new SqlParameter("@6", SqlDbType.DateTime2)
                            {
                                Value = s.ValidationDTG.GetDbValue()
                            });
                            cmd.Parameters.Add(new SqlParameter("@7", SqlDbType.NVarChar)
                            {
                                Value = s.AsciiContent.GetDbValue()
                            });
                            try
                            {
                                await cmd.ExecuteNonQueryAsync();
                            }
                            catch (Exception ex)
                            {

                                // throw;
                            }

                        }
                    }
                    
                }
            }
        }
    }
}
