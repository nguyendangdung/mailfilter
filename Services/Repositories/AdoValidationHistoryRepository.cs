using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.IRepository;

namespace Services.Repositories
{
    public class AdoValidationHistoryRepository : IValidationHistoryRepository
    {
        private readonly string _connectionString = ConfigurationManager.ConnectionStrings["ValidationContext"].ConnectionString;
        public Task<IEnumerable<ValidationHistory>> GetAllAsync(int page = 1, int size = 20)
        {
            throw new NotImplementedException();
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
                        "INSERT [dbo].[ValidationHistories]([ValidationHistoryID], [Content], [Status], [Description], [FileName], [EmailContentId], [ValidationDTG]) " +
                        "VALUES (@0, @1, @2, @3, @4, @5, @6)";
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
                                Value = s.Content
                            });
                            cmd.Parameters.Add(new SqlParameter("@2", SqlDbType.Int)
                            {
                                Value = (int)s.Status
                            });
                            cmd.Parameters.Add(new SqlParameter("@3", SqlDbType.NVarChar)
                            {
                                Value = s.Description
                            });
                            cmd.Parameters.Add(new SqlParameter("@4", SqlDbType.NVarChar)
                            {
                                Value = s.FileName
                            });
                            cmd.Parameters.Add(new SqlParameter("@5", SqlDbType.UniqueIdentifier)
                            {
                                Value = s.EmailContentId
                            });
                            cmd.Parameters.Add(new SqlParameter("@6", SqlDbType.DateTime2)
                            {
                                Value = s.ValidationDTG
                            });
                            
                            await cmd.ExecuteNonQueryAsync();
                        }
                    }
                    
                }
            }
        }
    }
}
