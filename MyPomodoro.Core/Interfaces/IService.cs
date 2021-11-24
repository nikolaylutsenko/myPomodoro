using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyPomodoro.Core.Interfaces
{
    public interface IService<T> : IDisposable
    {
        Task AddAsync(T model);
        Task UpdateAsync(T model);
        Task DeleteAsync(T model);
        Task<IEnumerable<T>> GetAllAsync();
    }
}