using ERPSystem.Models;

namespace ERPSystem.Repository.Base
{
    public interface ICustomerRepository : IGenericRepository<Customer>
    {
        Task<IEnumerable<Customer>> SearchAsync(string? search);
        Task<int> CountAsync();
    }
}
