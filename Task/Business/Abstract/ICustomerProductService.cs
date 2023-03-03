using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Task.Models;

namespace Task.Business.Abstract
{
    public interface ICustomerProductService : IEntityService<CustomerProduct>
    {
        void AddProductToCustomer(int customerId, int productId);
        void RemoveProductFromCustomer(int customerId, int productId);
    }
}