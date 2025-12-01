using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ZealTravel.Domain.Interfaces.IRepository
{
    public interface IRepositoryBase<T> : IDisposable where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> GetByIdAsync(int id);
        Task<T> AddAsync(T entity);
        Task<T> UpdateAsync(T entity);
        Task DeleteAsync(int id);
        Task<T> FindAsync(Expression<Func<T, bool>> predicate);
        Task<int> TotalRecordsAsync();
    }
}
