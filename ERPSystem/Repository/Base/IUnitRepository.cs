using ERPSystem.Models;

namespace ERPSystem.Repository.Base
{
    public interface IUnitRepository : IGenericRepository<Unit>
    {
        Task<Unit?> GetByNameAsync(string name);
        Task<IEnumerable<Unit>> SearchAsync(string search);
    }
}
