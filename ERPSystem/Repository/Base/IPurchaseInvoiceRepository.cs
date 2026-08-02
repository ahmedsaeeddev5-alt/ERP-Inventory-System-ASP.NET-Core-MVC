using ERPSystem.Models;
using ERPSystem.ViewModels.Dashboard;
using ERPSystem.ViewModels.Reports;

namespace ERPSystem.Repository.Base
{
    public interface IPurchaseInvoiceRepository : IGenericRepository<PurchaseInvoice>
    {

        Task<PurchaseInvoice?> GetByIdWithDetailsAsync(int id);

        Task<PurchaseInvoice?> GetByInvoiceNumberAsync(string invoiceNumber);
        Task<List<PurchaseInvoice>> GetAllWithDetailsAsync();
        Task<IEnumerable<PurchaseReportVM>> GetPurchaseReportAsync();
        Task<decimal> GetTotalPurchasesAsync();
        Task<List<MonthlySalesPurchaseVM>> GetMonthlyPurchasesAsync();
        Task<int> CountAsync();
    }
}
