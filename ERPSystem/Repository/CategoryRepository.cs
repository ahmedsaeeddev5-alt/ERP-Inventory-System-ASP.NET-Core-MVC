using ERPSystem.Data;
using ERPSystem.Models;
using ERPSystem.Repository.Base;
using Microsoft.EntityFrameworkCore;

namespace ERPSystem.Repository
{
    public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
    {
        protected readonly ERPDbContext _context;
        public CategoryRepository(ERPDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Category?> GetByNameAsync(string name)
        {
            return await _context.Categories.FirstOrDefaultAsync(c => c.Name == name);
        }

        public async Task<IEnumerable<Category>> SearchAsync(string? search)
        {
            var query = _context.Categories.AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(c =>
                    c.Name.Contains(search) ||
                    c.Description.Contains(search));
            }

            return await query.ToListAsync();
        }
    }
}
