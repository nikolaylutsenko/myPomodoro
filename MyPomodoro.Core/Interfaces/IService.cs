using System.Linq.Expressions;

namespace MyPomodoro.Core.Interfaces;

public interface IStorageService<T> : IDisposable
{
    Task AddAsync(T model);
    Task UpdateAsync(T model);
    Task DeleteAsync(T model);
    Task<IEnumerable<T>> GetAllAsync();
    Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> properties);
}