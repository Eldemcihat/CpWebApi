using Task.Models;

namespace Task.Abstract
{
    public interface IUserRepository : IEntityRepository<User>
    {
        User Add(UserRegister request);
        User GetUser(UserLogin request);
        void RoleAssignment(int Id, string role);
    }
}