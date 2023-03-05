using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.AspNetCore.Mvc;
using Task.Business.Abstract;
using Task.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;

namespace Task.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService customerService;
        private readonly IMemoryCache memoryCache;
        private readonly IConfiguration configuration;
        private const string CachePrefix = "Customer_";
        public CustomerController(ICustomerService customerService, IMemoryCache memoryCache, IConfiguration configuration)
        {
            this.customerService = customerService;
            this.memoryCache = memoryCache;
            this.configuration = configuration;
        }

        private string GenerateToken(Customer customer)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, customer.Name),
                new Claim(ClaimTypes.Email, customer.Mail),
                new Claim(ClaimTypes.MobilePhone, customer.Msisdn)
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.GetSection("AppSettings:Token").Value)),
                    SecurityAlgorithms.HmacSha512Signature
                )
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }

        [HttpPost("register")]
        public ActionResult<Customer> Register(CustomerRegister request)
        {
            var password = BCrypt.Net.BCrypt.HashPassword(request.Password);
            var customer = new Customer()
            {
                Name = request.Name,
                Password = password,
                Mail = request.Mail,
                Msisdn = request.Msisdn
            };

            var result = customerService.AddCustomer(request);
            var cacheKey = $"{CachePrefix}GetById_{result.Id}";
            memoryCache.Set(cacheKey, result, TimeSpan.FromMinutes(10));

            string token = GenerateToken(result);

            var response = new { Token = token };
            return Ok(response);
        }

        [HttpPost("login")]
        public ActionResult<Customer> Login(CustomerLogin request)
        {
            var cacheKey = $"{CachePrefix}GetCustomer_{request.Name}_{request.Password}";
            var result = memoryCache.GetOrCreate(cacheKey, entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10);
                return customerService.GetCustomer(request);
            });

            string token = GenerateToken(result);

            var response = new { Token = token };
            return Ok(response);
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
                return customers.Select(u => new Customer { Mail = u.Mail, Msisdn = u.Msisdn, Name = u.Name }).ToList();
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