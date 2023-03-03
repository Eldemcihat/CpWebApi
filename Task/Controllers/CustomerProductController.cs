using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Task.Business.Abstract;

namespace Task.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomerProductController : ControllerBase
    {
        ICustomerProductService customerProductService;
        public CustomerProductController(ICustomerProductService customerProductService)
        {
            this.customerProductService = customerProductService;
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("CustomerProductAdd")]
        public void AddProductToCustomer(int customerId, int productId)
        {
            customerProductService.AddProductToCustomer(customerId, productId);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("CustomerProductDelete")]
        public void RemoveProductToCustomer(int customerId, int productId)
        {
            customerProductService.RemoveProductFromCustomer(customerId, productId);
        }
    }
}