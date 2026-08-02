using ERPSystem.Data;
using ERPSystem.Models;
using ERPSystem.Repository.Base;
using Microsoft.EntityFrameworkCore;

namespace ERPSystem.Repository
{
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        protected readonly ERPDbContext _context;
        public ProductRepository(ERPDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<int> CountAsync()
        {
            return await _context.Products.CountAsync();
        }

        public async Task<IEnumerable<Product>> GetAllWithDetailsAsync()
        {
            return await _context.Products
               .Include(p => p.Category)
                .Include(p => p.Unit)
                 .ToListAsync();
        }

        public async Task<Product?> GetByIdWithDetailsAsync(int id)
        {
            return await _context.Products
              .Include(p => p.Category)
              .Include(p => p.Unit)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<Product?> GetByNameAsync(string name)
        {
            return await _context.Products.FirstOrDefaultAsync( p => p.Name == name);
        }

        public async Task<IEnumerable<Product>> SearchAsync(string? search)
        {
            var query = _context.Products
                .Include(p => p.Category)
                .Include(p => p.Unit)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(p =>
                    p.Name.Contains(search) ||
                    p.Code.Contains(search));
            }

            return await query.ToListAsync();
        }
    }
}
