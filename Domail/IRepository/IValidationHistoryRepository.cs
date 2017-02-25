using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;

namespace Domain.IRepository
{
    public interface IValidationHistoryRepository
    {
        Task<IEnumerable<ValidationHistory>> GetAllAsync(int page = 1, int size = 20);
        Task<int> AddAsync(ValidationHistory item);
    }
}
