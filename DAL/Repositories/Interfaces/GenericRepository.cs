using DAL.EF;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace DAL.Repositories.Interfaces
{
    public abstract class GenericRepository<T> : IDisposable, IGenericRepository<T> where T : class
    {
        protected ApplicationDbContext _context;

        public GenericRepository(IConfiguration configuration)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            optionsBuilder.UseSqlServer(configuration.GetConnectionString("DataConnection"));
            _context = new ApplicationDbContext(optionsBuilder.Options);
        }

        public IQueryable<T> GetAll()
        {
            return _context.Set<T>();
        }

        public virtual T Get(int id)
        {
            return _context.Set<T>().Find(id);
        }

        public virtual void Add(T t)
        {
            _context.Set<T>().Add(t);
            _context.SaveChanges();
        }

        public virtual void Delete(T entity)
        {
            _context.Set<T>().Remove(entity);
            _context.SaveChanges();
        }

        public virtual T Update(T t)
        {
            if (t == null)
                return null;
            T exist = _context.Set<T>().Find(t);
            if (exist != null)
            {
                _context.Entry(exist).CurrentValues.SetValues(t);
                _context.SaveChanges();
            }
            return exist;
        }

        public virtual void Save()
        {
            _context.SaveChanges();
        }

        public virtual IQueryable<T> FindBy(Expression<Func<T, bool>> predicate)
        {
            IQueryable<T> query = _context.Set<T>().Where(predicate);
            return query;
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
                this.disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
