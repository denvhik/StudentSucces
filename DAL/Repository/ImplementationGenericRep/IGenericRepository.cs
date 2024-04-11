namespace DAL.Repository.Implementation;
public interface IGenericRepository<T> where T : class
{
    Task<T> GetByIdAsync(int? id);
    Task<List<T>> GetAllAsync();
    Task<T> AddAsync(T entity);
    Task<T> UpdateAsync(T entity);
    Task<T> DeleteByIdAsync(int id);
    Task SaveChangesAsync();
}
