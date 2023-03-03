using Task.Abstract;
using Task.Business.Abstract;

namespace Task.Business.Concrete
{
    public class EntityManager<T> : IEntityService<T> where T : class
    {
        private readonly IEntityRepository<T> entityRepository;

        public EntityManager(IEntityRepository<T> entityRepository)
        {
            this.entityRepository = entityRepository;
        }
        public T Create(T entity)
        {
            return entityRepository.Add(entity);
        }

        public void Delete(int id)
        {
            entityRepository.Delete(id);
        }

        public IEnumerable<T> GetAll()
        {
            return entityRepository.GetAll();
        }

        public T GetById(int id)
        {
            return entityRepository.GetById(id);
        }

        public T Update(T entity)
        {
            return entityRepository.Update(entity);
        }
    }
}