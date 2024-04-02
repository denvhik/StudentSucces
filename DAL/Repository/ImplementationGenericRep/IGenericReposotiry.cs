namespace DAL.Repository.Implementation;
public interface IGenericReposotiry<T> where T : class
{
    Task<T> GetByIdAsync(int? id);
    Task<IEnumerable<T>> GetAllAsync();
    Task<T> AddAsync(T entity);
    Task<T> UpdateAsync(T entity);
    Task<T> DeleteByIdAsync(int id);
    Task SaveChangesAsync();

}
