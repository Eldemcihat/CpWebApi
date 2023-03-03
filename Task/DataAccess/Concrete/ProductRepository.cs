using Task.DataAccess.Concrete;

namespace Task.Abstract.Concrete
{
    public class ProductRepository : EntityRepository<Product>, IProductRepository
    {
        public ProductRepository(ProductCustomerContext productCustomerContext) : base(productCustomerContext)
        {
        }
    }
}