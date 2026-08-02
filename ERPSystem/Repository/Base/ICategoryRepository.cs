using ERPSystem.Models;

namespace ERPSystem.Repository.Base
{
    public interface ICategoryRepository : IGenericRepository<Category>
    {
         Task<Category?> GetByNameAsync(string name);
         Task<IEnumerable<Category>> SearchAsync(string? search);

    }
}
