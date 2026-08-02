using ERPSystem.Models;

namespace ERPSystem.Services
{
    public interface IPurchaseInvoiceService
    {
        Task<IEnumerable<PurchaseInvoice>> GetAllAsync();

        Task<PurchaseInvoice?> GetByIdAsync(int id);

        Task CreateAsync(PurchaseInvoice invoice);

        Task UpdateAsync(PurchaseInvoice invoice);

        Task DeleteAsync(int id);
    }
}
