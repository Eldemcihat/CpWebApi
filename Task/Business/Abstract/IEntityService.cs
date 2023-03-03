using System.Collections.Generic;

namespace Task.Business.Abstract
{
    public interface IEntityService<T>
    {
        IEnumerable<T> GetAll();

        T GetById(int id);

        T Create(T entity);

        T Update(T entity);

        void Delete(int id);
    }
}