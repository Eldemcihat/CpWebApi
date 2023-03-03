using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Task.Abstract;
using Task.Abstract.Concrete;
using Task.Business.Abstract;

namespace Task.Business
{
    public class ProductManager : IProductService
    {
        private readonly IEntityRepository<Product> entityRepository;
        public ProductManager(IEntityRepository<Product> entityRepository)
        {
            this.entityRepository = entityRepository;
        }
        public Product Create(Product entity)
        {
            return entityRepository.Add(entity);
        }

        public void Delete(int id)
        {
            entityRepository.Delete(id);
        }

        public IEnumerable<Product> GetAll()
        {
            return entityRepository.GetAll();
        }

        public Product GetById(int id)
        {
            return entityRepository.GetById(id);
        }

        public Product Update(Product entity)
        {
            return entityRepository.Update(entity);
        }
    }
}