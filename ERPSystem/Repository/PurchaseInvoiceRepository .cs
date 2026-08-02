using ERPSystem.Data;
using ERPSystem.Models;
using ERPSystem.Repository.Base;
using ERPSystem.ViewModels.Dashboard;
using ERPSystem.ViewModels.Reports;
using Microsoft.EntityFrameworkCore;


namespace ERPSystem.Repository
{
    public class PurchaseInvoiceRepository : GenericRepository<PurchaseInvoice>, IPurchaseInvoiceRepository
    {
        private readonly ERPDbContext _context;

        public PurchaseInvoiceRepository(ERPDbContext context)
            : base(context)
        {
            _context = context;
        }

        public async Task<int> CountAsync()
        {
            return await _context.PurchaseInvoices.CountAsync();
        }

        public async Task<List<PurchaseInvoice>> GetAllWithDetailsAsync()
        {
            return await _context.PurchaseInvoices
                .Include(p => p.Supplier)
                .Include(p => p.Warehouse)
                .Include(p => p.Items)
                    .ThenInclude(i => i.Product)
                .ToListAsync();
        }

        public async Task<PurchaseInvoice?> GetByIdWithDetailsAsync(int id)
        {
            return await _context.PurchaseInvoices
                .Include(p => p.Supplier)
                .Include(p => p.Warehouse)
                .Include(p => p.Items)
                    .ThenInclude(i => i.Product)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<PurchaseInvoice?> GetByInvoiceNumberAsync(string invoiceNumber)
        {
            return await _context.PurchaseInvoices
                .FirstOrDefaultAsync(p => p.InvoiceNumber == invoiceNumber);
        }

        public async Task<List<MonthlySalesPurchaseVM>> GetMonthlyPurchasesAsync()
        {
            var purchases = await _context.PurchaseInvoices
                .GroupBy(x => new
                {
                    Year = x.InvoiceDate.Year,
                    Month = x.InvoiceDate.Month
                })
                .Select(g => new MonthlySalesPurchaseVM
                {
                    Month = g.Key.Month.ToString(),
                    TotalPurchases = g.Sum(x => x.TotalAmount),
                    TotalSales = 0
                })
                .ToListAsync();


            var sales = await _context.SalesInvoices
                .GroupBy(x => new
                {
                    Year = x.InvoiceDate.Year,
                    Month = x.InvoiceDate.Month
                })
                .Select(g => new MonthlySalesPurchaseVM
                {
                    Month = g.Key.Month.ToString(),
                    TotalSales = g.Sum(x => x.TotalAmount),
                    TotalPurchases = 0
                })
                .ToListAsync();


            return sales
                .Concat(purchases)
                .GroupBy(x => x.Month)
                .Select(g => new MonthlySalesPurchaseVM
                {
                    Month = g.Key,
                    TotalSales = g.Sum(x => x.TotalSales),
                    TotalPurchases = g.Sum(x => x.TotalPurchases)
                })
                .OrderBy(x => int.Parse(x.Month))
                .ToList();
        }

        public async Task<IEnumerable<PurchaseReportVM>> GetPurchaseReportAsync()
        {
            return await _context.PurchaseInvoices
                .Include(x => x.Supplier)
                .Select(x => new PurchaseReportVM
                {
                    InvoiceNumber = x.InvoiceNumber,
                    Date = x.InvoiceDate,
                    SupplierName = x.Supplier.Name,
                    Total = x.TotalAmount
                })
                .ToListAsync();
        }

        public async Task<decimal> GetTotalPurchasesAsync()
        {
            return await _context.PurchaseInvoices
                .SumAsync(x => x.TotalAmount);
        }
    }
}
