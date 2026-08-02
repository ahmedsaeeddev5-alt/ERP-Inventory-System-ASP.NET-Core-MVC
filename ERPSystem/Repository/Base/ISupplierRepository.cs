using ERPSystem.Models;

namespace ERPSystem.Repository.Base
{
    public interface ISupplierRepository : IGenericRepository<Supplier>
    {
        Task<IEnumerable<Supplier>> SearchAsync(string? search);
        Task<int> CountAsync();
    }
}
