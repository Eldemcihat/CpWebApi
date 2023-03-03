using System;
using System.Collections.Generic;
using System.Linq;
using Task.Abstract;
using Task.DataAccess.Abstract;
using Task.Models;

namespace Task.DataAccess.Concrete
{
    public class CustomerProductRepository : EntityRepository<CustomerProduct>, ICustomerProductRepository
    {
        private readonly ProductCustomerContext _context;

        public CustomerProductRepository(ProductCustomerContext productCustomerContext) : base(productCustomerContext)
        {
            _context = productCustomerContext;
        }

        public void AddProductToCustomer(int customerId, int productId)
        {
            var customer = _context.Customers.Find(customerId);
            var product = _context.Products.Find(productId);

            if (customer == null || product == null)
            {
                throw new ArgumentException("Invalid customer or product ID.");
            }

            var customerProduct = new CustomerProduct
            {
                CustomerId = customerId,
                ProductId = productId
            };

            _context.CustomerProducts.Add(customerProduct);
            _context.SaveChanges();
        }

        public void RemoveProductFromCustomer(int customerId, int productId)
        {
            var customerProduct = _context.CustomerProducts.FirstOrDefault(cp => cp.CustomerId == customerId && cp.ProductId == productId);

            if (customerProduct == null)
            {
                throw new ArgumentException("Customer does not have this product.");
            }

            _context.CustomerProducts.Remove(customerProduct);
            _context.SaveChanges();
        }
    }
}