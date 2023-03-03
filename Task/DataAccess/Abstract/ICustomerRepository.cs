using Task.Abstract;
using Task.Models;

namespace Task.DataAccess.Abstract
{
    public interface ICustomerRepository : IEntityRepository<Customer>
    {
        Customer AddCustomer(CustomerRegister request);
        Customer GetCustomer(CustomerLogin request);
    }
}