using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MyPomodoro.Core.Interfaces;

namespace MyPomodoro.Dal
{
    public class Service<T> : IService<T> where T : class
    {
        private readonly AppContext _context;
        private readonly DbSet<T> _dbSet;

        public Service()
        {
            _context = new AppContext();
            _dbSet = _context.Set<T>();
        }

        public async Task AddAsync(T model)
        {
            await _dbSet.AddAsync(model);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(T model)
        {
            _dbSet.Update(model);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(T model)
        {
            _dbSet.Remove(model);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            var result = _dbSet.AsQueryable();
            return await result.ToListAsync();
        }

        private bool _disposed;

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }

            _disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}