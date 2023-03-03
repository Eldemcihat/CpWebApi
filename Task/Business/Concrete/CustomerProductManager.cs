using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Task.Abstract;
using Task.Business.Abstract;
using Task.DataAccess.Abstract;
using Task.Models;

namespace Task.Business.Concrete
{
    public class CustomerProductManager : ICustomerProductService
    {
        private readonly ICustomerProductRepository customerProductRepository;
        private readonly IEntityRepository<CustomerProduct> entityRepository;

        public CustomerProductManager(IEntityRepository<CustomerProduct> entityRepository, ICustomerProductRepository customerProductRepository)
        {
            this.customerProductRepository = customerProductRepository;
            this.entityRepository = entityRepository;
        }
        public void AddProductToCustomer(int customerId, int productId)
        {
            customerProductRepository.AddProductToCustomer(customerId, productId);
        }

        public CustomerProduct Create(CustomerProduct entity)
        {
            return entityRepository.Add(entity);
        }

        public void Delete(int id)
        {
            entityRepository.Delete(id);
        }

        public IEnumerable<CustomerProduct> GetAll()
        {
            return entityRepository.GetAll();
        }

        public CustomerProduct GetById(int id)
        {
            return entityRepository.GetById(id);
        }

        public void RemoveProductFromCustomer(int customerId, int productId)
        {
            customerProductRepository.RemoveProductFromCustomer(customerId, productId);
        }

        public CustomerProduct Update(CustomerProduct entity)
        {
            return entityRepository.Update(entity);
        }
    }
}