using System.Linq.Expressions;
using DataAccessLayer.Context;
using DataAccessLayer.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Repositories.Implemantations;

public class Repository<T> : IRepository<T> where T : class
{
    public readonly ApplicationDbContext _context;
    public readonly DbSet<T> _dbSet;

    public Repository(ApplicationDbContext context)
    {
        _context = context;
        _dbSet = context.Set<T>();
    }
    
    public async Task<T?> GetByAsyncId(int id)
    {
        var entity = await _dbSet.FindAsync(id);
        return entity;
    }

    public async Task<List<T?>?> GetAllAsync()
    {
        List<T?> list = (await _dbSet.ToListAsync())!;
        return list;
    }

    public async Task AddAsync(T entity)
    {
        await _dbSet.AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(T entity)
    {
        _dbSet.Update(entity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var entity = await GetByAsyncId(id);
        _dbSet.Remove(entity);
        await _context.SaveChangesAsync();
    }

    public async Task<List<T>> FindAsync(Expression<Func<T, bool>> predicate)
    {
        return await _dbSet.Where(predicate).ToListAsync();
    }

    public async Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate)
    {
        var list = await _dbSet.Where(predicate).ToListAsync();
        return list.Count > 0;
    }
}