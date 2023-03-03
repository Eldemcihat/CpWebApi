using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Task.Business;

namespace Task.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IProductService productService;

        public ProductController(IProductService productService)
        {
            this.productService = productService;
        }

        [HttpGet("GetAll")]
        public IEnumerable<Product> Get()
        {
            return productService.GetAll();
        }

        [HttpGet("GetById{id}")]
        public Product Get(int id)
        {
            return productService.GetById(id);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("Update")]
        public Product Update([FromBody] Product entity)
        {
            return productService.Update(entity);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("Create")]
        public Product Post([FromBody] Product entity)
        {
            return productService.Create(entity);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("Delete{id}")]
        public void Delete(int id)
        {
            productService.Delete(id);
        }
    }
}