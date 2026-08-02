using System.Linq.Expressions;

namespace ERPSystem.Repository.Base
{
    public interface IGenericRepository<T> where T : class
    {
     
        Task<IEnumerable<T>> GetAllAsync();
        Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate); //ووظيفته هي إرجاع جميع العناصر التي تحقق شرطًا معينًا بشكل غير متزامن

        Task<T?> GetByIdAsync(int id);

        Task AddAsync(T entity);

        void Update(T entity);

        void Delete(T entity);

        Task<bool> ExistsAsync(int id);

    }
}
