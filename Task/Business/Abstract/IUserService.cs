using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Task.Models;

namespace Task.Business.Abstract
{
    public interface IUserService : IEntityService<User>
    {
        User Add(UserRegister request);
        User GetUser(UserLogin request);
        void RoleAssignment(int Id, string role);
    }
}