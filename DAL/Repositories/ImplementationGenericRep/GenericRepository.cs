using Microsoft.EntityFrameworkCore;
using DAL.Models;
using Handling;
using System.Linq.Expressions;

namespace DAL.Repository.Implementation;
public class GenericRepository<T> : IGenericRepository<T> where T : class
{
    private readonly StudentSuccesContext _studentSuccesContext;
    private readonly DbSet<T> _dbSet;

    public GenericRepository(StudentSuccesContext studentSuccesContext)
    {
        _studentSuccesContext = studentSuccesContext;
        _dbSet = _studentSuccesContext.Set<T>();
    }
    public async Task<T> AddAsync(T entity)
    {
        try
        {
            await _dbSet.AddAsync(entity);
            await _studentSuccesContext.SaveChangesAsync();
            return entity;
        }
        catch (Exception ex)
        {
            throw SystemExeptionHandle.FromSystemException(ex);
        }
    }

    public async Task<T> DeleteByIdAsync(int id)
    {
        try
        {
            var deleted = await _dbSet.FindAsync(id);
            if (deleted != null)
            {
                _studentSuccesContext.Remove(deleted);
                await _studentSuccesContext.SaveChangesAsync();
            }
            return deleted;
        }
        catch (Exception ex)
        {
            throw SystemExeptionHandle.FromSystemException(ex);
        }
    }
    public async Task<List<T>> GetAllAsync()
    {
        try
        {
            var result = await _dbSet.ToListAsync();
            return result;
        }
        catch (Exception ex)
        {
            throw SystemExeptionHandle.FromSystemException(ex);
        }
    }

    public async Task<List<T>> GetPagingAsync(int skip, int take)
    {
        try
        {
            var result = await _dbSet.Skip(skip).Take(take).ToListAsync();
            return result;
        }
        catch (Exception ex)
        {
            throw SystemExeptionHandle.FromSystemException(ex);
        }
    }

    public async Task<T> GetByIdAsync(int? id)
    {
        try
        {
            var objectOf = await _dbSet.FindAsync(id);
            return objectOf;
        }
        catch (Exception ex)
        {
            throw SystemExeptionHandle.FromSystemException(ex);
        }
    }
    public async Task SaveChangesAsync()
    {
        await _studentSuccesContext.SaveChangesAsync();
    }
    
    public async Task<T> UpdateAsync(T entity)
    {
        try
        {
             _dbSet.Update(entity);
            await _studentSuccesContext.SaveChangesAsync();
            return entity;
        } 
        catch (Exception ex) 
        {
            throw SystemExeptionHandle.FromSystemException(ex);
        }
    }
    public async Task <List<T>> GetEntityById(Expression<Func<T, bool>> func, params string[] includes)
    {
        try
        {

            IQueryable<T> query = _studentSuccesContext.Set<T>().AsQueryable();

            // Динамічно додаємо включення на основі параметрів, переданих у запит
            foreach (var include in includes)
            {
                query = query.Include(include);
            }

            query = query.Where(func).AsNoTracking();

            return await query.ToListAsync();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
}
