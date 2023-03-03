using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Task.Abstract;
using Task.Business.Abstract;

namespace Task.Business
{
    public interface IProductService : IEntityService<Product>
    {

    }
}