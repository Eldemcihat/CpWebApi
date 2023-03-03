using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Task.Abstract;
using Task;

namespace Task.DataAccess.Concrete
{
    public class EntityRepository<T> : IEntityRepository<T> where T : class
    {
        protected ProductCustomerContext productCustomerContext;
        public EntityRepository(ProductCustomerContext productCustomerContext)
        {
            this.productCustomerContext = productCustomerContext;
        }
        public T Add(T entity)
        {
            productCustomerContext.Set<T>().Add(entity);
            productCustomerContext.SaveChanges();
            return entity;
        }

        public void Delete(int id)
        {
            productCustomerContext.Remove(GetById(id));
            productCustomerContext.SaveChanges();
        }


        public IEnumerable<T> GetAll()
        {
            return productCustomerContext.Set<T>().ToList();
        }


        public T GetById(int id)
        {
            T entity = productCustomerContext.Find<T>(id);
            if (entity == null)
            {
                throw new Exception($"Entity of type {typeof(T)} with ID {id} was not found.");
            }
            return entity;
        }

        public T Update(T entity)
        {
            productCustomerContext.Entry(entity).State = EntityState.Modified;
            productCustomerContext.SaveChanges();
            return entity;
        }
    }
}