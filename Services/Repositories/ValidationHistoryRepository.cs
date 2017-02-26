using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.IRepository;

namespace Services.Repositories
{
    public class ValidationHistoryRepository : IValidationHistoryRepository
    {
        private ValidationContext _context;

        public ValidationHistoryRepository()
        {
            _context = new ValidationContext();
        }

        public Task<IEnumerable<ValidationHistory>> GetAllAsync(int page = 1, int size = 20)
        {
            throw new NotImplementedException();
        }

        public async Task<int> AddAsync(ValidationHistory item)
        {
            _context.ValidationHistories.Add(item);
            return await _context.SaveChangesAsync();
        }
    }
}
