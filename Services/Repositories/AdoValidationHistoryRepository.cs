using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.IRepository;

namespace Services.Repositories
{
    public class AdoValidationHistoryRepository : IValidationHistoryRepository
    {
        public Task<IEnumerable<ValidationHistory>> GetAllAsync(int page = 1, int size = 20)
        {
            throw new NotImplementedException();
        }

        public Task<int> AddAsync(ValidationHistory item)
        {
            throw new NotImplementedException();
        }

        public Task<int> AddRangeAsync(List<ValidationHistory> items)
        {
            throw new NotImplementedException();
        }
    }
}
