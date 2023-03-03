namespace Task.Abstract
{
    public interface IEntityRepository<T> where T : class
    {
        T GetById(int id);
        IEnumerable<T> GetAll();

        T Add(T entity);
        T Update(T entity);
        void Delete(int id);
    }
}