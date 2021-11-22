using System;
using System.Threading.Tasks;

namespace MyPomodoro.Dal
{
    public interface IGenericRepository : IDisposable
    {
        T Insert<T>(T model);
        T Update<T>(T model);
        bool Delete<T>(T model);
        T Select<T>(int pk) where T : new();
        T[] SelectAll<T>() where T : new();
    }
    
    public class GenericRepository : IDisposable
    {
        private readonly AppContext _context;

        public GenericRepository()
        {
            _context = new AppContext();
        }
 
        public async Task<T> Insert<T>(T model)
        {
            var iRes = await _context.AddAsync(model);
            return model;
        }
 
        public T Update<T>(T model)
        {
            int iRes = Context.Update(model);
            return model;
        }
 
        public bool Delete<T>(T model)
        {
            int iRes = Context.Delete(model);
            return iRes.Equals(1);
        }
 
        public T Select<T>(int pk) where T : new()
        {
            var map = Context.GetMapping(typeof(T));
            return Context.Query<T>(map.GetByPrimaryKeySql, pk).First();
        }
 
        public void SelectAlls()
        {
            Context.Table<People>().ToArray();
        }
 
        public T[] SelectAll<T>() where T : new()
        {
            return new TableQuery<T>(Context).ToArray();
        }
     
        private bool disposed = false;
 
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    context.Dispose();
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