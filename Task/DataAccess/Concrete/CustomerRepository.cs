using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Task.DataAccess.Abstract;
using Task.Models;

namespace Task.DataAccess.Concrete
{
    public class CustomerRepository : EntityRepository<Customer>, ICustomerRepository
    {
        private readonly IConfiguration configuration;
        public CustomerRepository(ProductCustomerContext productCustomerContext, IConfiguration configuration) : base(productCustomerContext)
        {
            this.configuration = configuration;
        }

        public Customer AddCustomer(CustomerRegister request)
        {
            var password = BCrypt.Net.BCrypt.HashPassword(request.Password);

            var customer = new Customer()
            {
                Name = request.Name,
                Password = password,
                Mail = request.Mail,
                Msisdn = request.Msisdn,
            };
            productCustomerContext.Customers.Add(customer);
            productCustomerContext.SaveChanges();
            return customer;
        }

        public Customer GetCustomer(CustomerLogin request)
        {
            var customer = productCustomerContext.Customers.FirstOrDefault(u => u.Name == request.Name);

            if (customer == null || !BCrypt.Net.BCrypt.Verify(request.Password, customer.Password))
            {
                return null;
            }

            return customer;
        }
    }
}