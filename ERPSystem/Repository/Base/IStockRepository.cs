using ERPSystem.Models;
using ERPSystem.ViewModels.Dashboard;
using ERPSystem.ViewModels.Reports;

namespace ERPSystem.Repository.Base
{
    public interface IStockRepository : IGenericRepository<Stock>
    {
        Task<IEnumerable<Stock>> SearchAsync(string? search);
        Task<IEnumerable<Stock>> GetAllWithDetailsAsync();

        Task<Stock?> GetByIdWithDetailsAsync(int id);
        Task<Stock?> GetByProductAndWarehouseAsync(int productId, int warehouseId);
        Task<List<InventoryReportVM>> GetInventoryReportAsync();
        Task<List<LowStockVM>> GetLowStockProductsAsync();
        Task<decimal> GetStockValueAsync();

    }
}
