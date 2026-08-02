using ERPSystem.Data;
using ERPSystem.Models;
using ERPSystem.Repository.Base;
using ERPSystem.ViewModels.Dashboard;
using ERPSystem.ViewModels.Reports;
using Microsoft.EntityFrameworkCore;

namespace ERPSystem.Repository
{
    public class SalesInvoiceRepository : GenericRepository<SalesInvoice>, ISalesInvoiceRepository
    {
        public SalesInvoiceRepository(ERPDbContext context)
            : base(context)
        {
        }

        public async Task<int> CountAsync()
        {
            return await _context.SalesInvoices.CountAsync();
        }

        public async Task<IEnumerable<SalesInvoice>> GetAllWithDetailsAsync()
        {
            return await _context.SalesInvoices
                .Include(x => x.Customer)
                .Include(x => x.Items)
                .ThenInclude(x => x.Product)
                .ToListAsync();
        }

        public async Task<SalesInvoice?> GetByIdWithDetailsAsync(int id)
        {
            return await _context.SalesInvoices
               .Include(x => x.Customer)
               .Include(x => x.Items)
               .ThenInclude(x => x.Product)
               .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<List<MonthlySalesPurchaseVM>> GetMonthlySalesAsync()
        {
            var data = await _context.SalesInvoices
                .GroupBy(x => new
                {
                    x.InvoiceDate.Year,
                    x.InvoiceDate.Month
                })
                .Select(g => new
                {
                    Year = g.Key.Year,
                    Month = g.Key.Month,
                    TotalSales = g.Sum(x => x.TotalAmount)
                })
                .OrderBy(x => x.Year)
                .ThenBy(x => x.Month)
                .ToListAsync();


            return data.Select(x => new MonthlySalesPurchaseVM
            {
                Month = new DateTime(
                    x.Year,
                    x.Month,
                    1
                ).ToString("MMM"),

                TotalSales = x.TotalSales,

                TotalPurchases = 0

            }).ToList();
        }

        public async Task<List<SalesReportVM>> GetSalesReportAsync()
        {
            return await _context.SalesInvoiceItems
                .Include(x => x.SalesInvoice)
                    .ThenInclude(x => x.Customer)
                .Include(x => x.Product)

                .Select(x => new SalesReportVM
                {
                    InvoiceNumber = x.SalesInvoice.Id,

                    InvoiceDate = x.SalesInvoice.InvoiceDate,

                    CustomerName = x.SalesInvoice.Customer.Name,

                    ProductName = x.Product.Name,

                    Quantity = x.Quantity,

                    UnitPrice = x.UnitPrice
                })

                .ToListAsync();
        }

        public async Task<decimal> GetTotalSalesAsync()
        {
            return await _context.SalesInvoices
                .SumAsync(x => x.TotalAmount);
        }
    }
}
