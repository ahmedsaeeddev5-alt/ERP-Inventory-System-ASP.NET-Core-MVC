using ERPSystem.Data;
using ERPSystem.Models;
using ERPSystem.Repository.Base;
using ERPSystem.ViewModels.Dashboard;
using ERPSystem.ViewModels.Reports;
using Microsoft.EntityFrameworkCore;

namespace ERPSystem.Repository
{
    public class StockRepository : GenericRepository<Stock>, IStockRepository
    {
        protected readonly ERPDbContext _context;
        public StockRepository(ERPDbContext context) : base(context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Stock>> GetAllWithDetailsAsync()
        {
            return await _context.Stocks
                .Include(S=> S.Product)
                .Include(S=> S.Warehouse)
                .ToListAsync();
        }

        public async Task<Stock?> GetByIdWithDetailsAsync(int id)
        {
            return await _context.Stocks
                .Include(s => s.Product)
                .Include(s => s.Warehouse)
                .FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<Stock?> GetByProductAndWarehouseAsync(int productId, int warehouseId)
        {
            return await _context.Stocks
                .Include(s => s.Product)
                .Include(s => s.Warehouse)
                .FirstOrDefaultAsync(s =>
                    s.ProductId == productId &&
                    s.WarehouseId == warehouseId);
        }

        public async Task<List<InventoryReportVM>> GetInventoryReportAsync()
        {
            return await _context.Stocks
                .Include(s => s.Product)
                    .ThenInclude(p => p.Category)
                .Include(s => s.Product)
                    .ThenInclude(p => p.Unit)
                .Include(s => s.Warehouse)
                .Select(s => new InventoryReportVM
                {
                    ProductName = s.Product.Name,
                    CategoryName = s.Product.Category.Name,
                    UnitName = s.Product.Unit.Name,
                    WarehouseName = s.Warehouse.Name,

                    Quantity = s.Quantity,

                    PurchasePrice = s.Product.PurchasePrice
                })
                .ToListAsync();
        }

        public async Task<List<LowStockVM>> GetLowStockProductsAsync()
        {
            return await _context.Stocks
        .Where(x => x.Quantity <= 100)
        .Select(x => new LowStockVM
        {
            ProductName = x.Product.Name,

            Quantity = x.Quantity
        })
        .ToListAsync();
        }

        public async Task<decimal> GetStockValueAsync()
        {
            return await _context.Stocks
                .SumAsync(x => x.Quantity * x.Product.PurchasePrice);
        }

        public async Task<IEnumerable<Stock>> SearchAsync(string? search)
        {
            var query = _context.Stocks
                .Include(s => s.Product)
                .Include(s => s.Warehouse)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(s =>
                    s.Product.Name.ToLower().Contains(search.ToLower()) ||
                   s.Warehouse.Name.ToLower().Contains(search.ToLower()));
            }

            return await query.ToListAsync();
        }
    }
}
