using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ZealTravel.Domain.Interfaces.IRepository;
using ZealTravel.Domain.Data.Entities;
using ZealTravelWebsite.Infrastructure.Context;

public class RepositoryBase<T> : IRepositoryBase<T> where T : class
{
    private readonly ZealdbNContext _context;
    private readonly DbSet<T> _dbSet;

    public RepositoryBase(ZealdbNContext context)
    {
        _context = context;
        _dbSet = _context.Set<T>();
    }

    public async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _dbSet.ToListAsync();
    }

    public async Task<T> GetByIdAsync(int id)
    {
        return await _dbSet.FindAsync(id);
    }

    public async Task<T> AddAsync(T entity)
    {
        await _dbSet.AddAsync(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task<T> UpdateAsync(T entity)
    {
        var entry = _context.Entry(entity); 
        _dbSet.Attach(entity);
        entry.State = EntityState.Modified;
        entry.Property("Id").IsModified = false;
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task DeleteAsync(int id)
    {
        var entity = await _dbSet.FindAsync(id);
        if (entity != null)
        {
            _dbSet.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<T> FindAsync(Expression<Func<T, bool>> predicate)
    {
        return await _dbSet.FirstOrDefaultAsync(predicate);
    }

    public async Task<int> TotalRecordsAsync()
    {
        return await _dbSet.CountAsync();
    }

    public void Dispose()
    {
        _context?.Dispose();
    }
}
