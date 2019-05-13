using System.Collections.Generic;

namespace Identity.Services.Interfaces
{
    public interface IUserService<T> where T : class
    {
        void Add(T t);
        void Update(T t);
        void Delete(T entity);
        IEnumerable<T> GetAll();
        T Get(int id);
        void Save();
    }
}
