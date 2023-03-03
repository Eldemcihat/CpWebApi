using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Task.Abstract;
using Task.Models;

namespace Task.DataAccess.Abstract
{
    public interface ICustomerProductRepository : IEntityRepository<CustomerProduct>
    {
        void AddProductToCustomer(int customerId, int productId);
        void RemoveProductFromCustomer(int customerId, int productId);
    }
}