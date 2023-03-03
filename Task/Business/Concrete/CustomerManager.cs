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
    public class CustomerManager : ICustomerService
    {
        private readonly IEntityRepository<Customer> entityRepository;
        private readonly ICustomerRepository customerRepository;
        public CustomerManager(IEntityRepository<Customer> entityRepository, ICustomerRepository customerRepository)
        {
            this.entityRepository = entityRepository;
            this.customerRepository = customerRepository;
        }
        public Customer AddCustomer(CustomerRegister request)
        {
            if (request.Msisdn.Length != 11)
            {
                throw new ArgumentException("Phone number should be bigger than 11 digit");
            }

            if (!IsValidEmail(request.Mail))
            {
                throw new ArgumentException("Invalid mail address");
            }

            if (request.Password.Length < 6)
            {
                throw new ArgumentException("Password must bigger then 6 digit");
            }

            var customer = new Customer
            {
                Msisdn = request.Msisdn,
                Mail = request.Mail,
                Password = request.Password,
            };

            return customerRepository.AddCustomer(request);
        }
        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        public Customer Create(Customer entity)
        {
            return entityRepository.Add(entity);
        }

        public void Delete(int id)
        {
            entityRepository.Delete(id);
        }

        public IEnumerable<Customer> GetAll()
        {
            return entityRepository.GetAll();
        }

        public Customer GetById(int id)
        {
            return entityRepository.GetById(id);
        }

        public Customer GetCustomer(CustomerLogin request)
        {
            return customerRepository.GetCustomer(request);
        }

        public Customer Update(Customer entity)
        {
            return entityRepository.Update(entity);
        }
    }
}