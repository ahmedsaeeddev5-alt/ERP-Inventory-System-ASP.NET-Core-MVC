using ERPSystem.Data;
using ERPSystem.Models;
using ERPSystem.Repository.Base;
using Microsoft.EntityFrameworkCore;

namespace ERPSystem.Repository
{
    public class UnitRepository : GenericRepository<Unit>, IUnitRepository
    {
        protected readonly ERPDbContext _context;
        public UnitRepository(ERPDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Unit?> GetByNameAsync(string name)
        {
            return await _context.Units
                .FirstOrDefaultAsync(u => u.Name.ToLower() == name.ToLower());
        }
        public async Task<IEnumerable<Unit>> SearchAsync(string search)
        {
            return await _context.Units
                .Where(u => u.Name.Contains(search))
                .ToListAsync();
        }
    }
}
