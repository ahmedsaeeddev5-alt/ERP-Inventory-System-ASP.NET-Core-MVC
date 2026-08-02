using ERPSystem.Models;

namespace ERPSystem.Repository.Base
{
    public interface IProductRepository : IGenericRepository<Product>
    {
        Task<Product?> GetByNameAsync(string name);
        Task<IEnumerable<Product>> SearchAsync(string? search);
        Task<IEnumerable<Product>> GetAllWithDetailsAsync();

        Task<Product?> GetByIdWithDetailsAsync(int id);
        Task<int> CountAsync();
    }
}
