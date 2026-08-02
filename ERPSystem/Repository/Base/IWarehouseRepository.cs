using ERPSystem.Models;

namespace ERPSystem.Repository.Base
{
    public interface IWarehouseRepository : IGenericRepository<Warehouse>
    {
        Task<Warehouse?> GetByNameAsync(string name);
        Task<IEnumerable<Warehouse>> SearchAsync(string? search);
        Task<IEnumerable<Warehouse>> GetAllWithDetailsAsync();
        Task<Warehouse?> GetByIdWithDetailsAsync(int id);
    }
}
