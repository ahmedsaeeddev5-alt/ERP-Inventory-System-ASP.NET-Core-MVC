using ERPSystem.Data;
using ERPSystem.Models;
using ERPSystem.Repository.Base;
using Microsoft.EntityFrameworkCore;

namespace ERPSystem.Repository
{
    public class CustomerRepository : GenericRepository<Customer>, ICustomerRepository
    {
        public CustomerRepository(ERPDbContext context) : base(context)
        {
        }

        public async Task<int> CountAsync()
        {
            return await _context.Customers.CountAsync();
        }

        public async Task<IEnumerable<Customer>> SearchAsync(string? search)
        {
            var query = _context.Customers.AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(s =>
                    s.Name.Contains(search) ||
                    (s.Phone != null && s.Phone.Contains(search)) ||
                    (s.Email != null && s.Email.Contains(search)));
            }

            return await query.ToListAsync();
        }
    }
}
