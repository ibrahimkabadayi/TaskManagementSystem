using System.Linq.Expressions;

namespace DomainLayer.Interfaces;

public interface IRepository<T> where T : class
{
    Task<T?> GetByAsyncId(int id);
    Task<List<T>?> GetAllAsync();
    Task AddAsync(T entity);
    Task UpdateAsync(T entity);
    Task DeleteAsync(int id);
    Task<List<T>> FindAsync(Expression<Func<T, bool>> predicate);
    Task<T?> FindFirstAsync(Expression<Func<T, bool>> predicate);
    Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate);
}