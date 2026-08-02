using ERPSystem.Data;
using ERPSystem.Models;
using ERPSystem.Repository.Base;
using Microsoft.EntityFrameworkCore;

namespace ERPSystem.Repository
{
    public class WarehouseRepository : GenericRepository<Warehouse> , IWarehouseRepository
    {
        protected readonly ERPDbContext _context;
        public WarehouseRepository(ERPDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Warehouse>> GetAllWithDetailsAsync()
        {
            return await _context.Warehouses
               .Include(p => p.Stocks)
                 .ToListAsync();
        }

        public async Task<Warehouse?> GetByIdWithDetailsAsync(int id)
        {
            return await _context.Warehouses
              .Include(p => p.Stocks)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<Warehouse?> GetByNameAsync(string name)
        {
            return await _context.Warehouses.FirstOrDefaultAsync(p => p.Name == name);
        }

        public async Task<IEnumerable<Warehouse>> SearchAsync(string? search)
        {
            var query = _context.Warehouses
                .Include(p => p.Stocks)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(p =>
                  p.Name.Contains(search) ||
                  (p.Address != null && p.Address.Contains(search)));
            }

            return await query.ToListAsync();
        }
    }
}

