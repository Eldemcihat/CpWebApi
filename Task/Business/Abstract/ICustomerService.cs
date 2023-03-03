using Task.Models;

namespace Task.Business.Abstract
{
    public interface ICustomerService : IEntityService<Customer>
    {
        Customer AddCustomer(CustomerRegister request);
        Customer GetCustomer(CustomerLogin request);
    }
}