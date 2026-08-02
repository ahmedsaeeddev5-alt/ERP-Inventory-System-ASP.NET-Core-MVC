using ERPSystem.Models;
using ERPSystem.ViewModels.Dashboard;
using ERPSystem.ViewModels.Reports;

namespace ERPSystem.Repository.Base
{
    public interface ISalesInvoiceRepository : IGenericRepository<SalesInvoice>
    {
        Task<IEnumerable<SalesInvoice>> GetAllWithDetailsAsync();

        Task<SalesInvoice?> GetByIdWithDetailsAsync(int id);
        Task<List<SalesReportVM>> GetSalesReportAsync();
        Task<decimal> GetTotalSalesAsync();
        Task<List<MonthlySalesPurchaseVM>> GetMonthlySalesAsync();
        Task<int> CountAsync();
    }
}
