using System.Linq.Expressions;

namespace DAL.Repository.Implementation;
public interface IGenericRepository<T> where T : class
{
    Task<T> GetByIdAsync(int? id);
    Task<List<T>> GetAllAsync();
    Task<T> AddAsync(T entity);
    Task<T> UpdateAsync(T entity);
    Task<T> DeleteByIdAsync(int id);
    Task SaveChangesAsync();
    Task<List<T>> GetPagingAsync(int skip , int take);
    Task<List<T>>GetEntityById(Expression<Func<T, bool>> func, params string[] includes);
}
