using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.AspNetCore.Mvc;
using Task.Business.Abstract;
using Task.Models;
using Microsoft.AspNetCore.Authorization;

namespace Task.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService customerService;
        private readonly IMemoryCache memoryCache;
        private const string CachePrefix = "Customer_";
        public CustomerController(ICustomerService customerService, IMemoryCache memoryCache)
        {
            this.customerService = customerService;
            this.memoryCache = memoryCache;
        }

        [HttpPost("CustomerRegister")]
        public ActionResult<Customer> Register(CustomerRegister request)
        {
            var result = customerService.AddCustomer(request);
            var cacheKey = $"{CachePrefix}GetById_{result.Id}";
            memoryCache.Set(cacheKey, result, TimeSpan.FromMinutes(10));
            return result;
        }

        [HttpPost("CustomerLogin")]
        public ActionResult<Customer> login(CustomerLogin request)
        {
            var cacheKey = $"{CachePrefix}GetCustomer_{request.Name}_{request.Password}";
            var result = memoryCache.GetOrCreate(cacheKey, entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10);
                return customerService.GetCustomer(request);
            });

            return result;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("GetAllCustomer")]
        public IEnumerable<Customer> Get()
        {

            var cacheKey = $"{CachePrefix}GetAllCustomer";
            var result = memoryCache.GetOrCreate(cacheKey, entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10);
                var customers = customerService.GetAll();
                return customers.Select(u => new Customer { Mail = u.Mail, Msisdn = u.Msisdn, Name = u.Name, Token = u.Token, Password = u.Password }).ToList();
            });

            return result;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("GetByIdCustomer{id}")]
        public Customer Get(int id)
        {
            var cacheKey = $"{CachePrefix}GetByIdCustomer_{id}";
            var result = memoryCache.GetOrCreate(cacheKey, entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10);
                return customerService.GetById(id);
            });

            return result;
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("Update")]
        public Customer Update([FromBody] Customer entity)
        {
            var cacheKey = $"{CachePrefix}UpdateCustomer_{entity.Id}";
            var result = customerService.Update(entity);
            memoryCache.Set(cacheKey, result, TimeSpan.FromMinutes(10));
            return result;
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("DeleteCustomer{id}")]
        public void Delete(int id)
        {
            var cacheKey = $"{CachePrefix}DeleteCustomer_{id}";
            customerService.Delete(id);
            memoryCache.Remove(cacheKey);
        }
    }
}