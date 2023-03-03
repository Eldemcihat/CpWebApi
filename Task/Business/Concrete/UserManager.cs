using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Task.Abstract;
using Task.Business.Abstract;
using Task.DataAccess.Concrete;
using Task.Models;

namespace Task.Business.Concrete
{
    public class UserManager : IUserService
    {
        private readonly IEntityRepository<User> entityRepository;
        private readonly IUserRepository userRepository;
        public UserManager(IEntityRepository<User> entityRepository, IUserRepository userRepository)
        {
            this.entityRepository = entityRepository;
            this.userRepository = userRepository;
        }

        public User Add(UserRegister request)
        {
            if (request.Msisdn.Length != 11)
            {
                throw new ArgumentException("Phone number should be bigger than 11 digit");
            }

            if (!IsValidEmail(request.Mail))
            {
                throw new ArgumentException("Invalid mail address");
            }

            if (request.Password.Length < 6)
            {
                throw new ArgumentException("Password must bigger then 6 digit");
            }

            var user = new User
            {
                Msisdn = request.Msisdn,
                Mail = request.Mail,
                Password = request.Password,
            };

            return userRepository.Add(request);
        }

        public User GetUser(UserLogin request)
        {
            return userRepository.GetUser(request);
        }

        public User Create(User entity)
        {
            return entityRepository.Add(entity);
        }

        public void Delete(int id)
        {
            entityRepository.Delete(id);
        }

        public IEnumerable<User> GetAll()
        {
            return entityRepository.GetAll();
        }

        public User GetById(int id)
        {
            return entityRepository.GetById(id);
        }

        public User Update(User entity)
        {
            if (entity.Msisdn.Length != 11)
            {
                throw new ArgumentException("Phone number should be bigger than 11 digit");
            }

            if (!IsValidEmail(entity.Mail))
            {
                throw new ArgumentException("Invalid mail address");
            }

            return entityRepository.Update(entity);
        }
        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        public void RoleAssignment(int Id, string role)
        {
            userRepository.RoleAssignment(Id, role);
        }

        public void RoleAssignment(string Name, string role)
        {
            throw new NotImplementedException();
        }
    }
}