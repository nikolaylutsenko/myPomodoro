using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MyPomodoro.Core.Entities;

namespace MyPomodoro.Dal
{
    public interface IGenericRepository<T> : IDisposable
    {
        Task Insert(T model);
        Task Update(T model);
        Task Delete(T model);
        Task<IEnumerable<T>> List();
    }
    
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly AppContext _context;
        private readonly DbSet<T> _dbSet;

        public GenericRepository()
        {
            _context = new AppContext();
            _dbSet = _context.Set<T>();
        }
 
        public async Task Insert(T model)
        {
            try
            {
                await _context.Pomodoros.AddAsync(model as Pomodoro);
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
 
        public async Task Update(T model)
        {
            _dbSet.Update(model);
            await _context.SaveChangesAsync();
        }
 
        public async Task Delete(T model)
        {
            _dbSet.Remove(model);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<T>> List()
        {
            var result = _dbSet.AsQueryable();
            return await result.ToListAsync();
        }

        private bool disposed = false;
 
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            this.disposed = true;
        }
 
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}