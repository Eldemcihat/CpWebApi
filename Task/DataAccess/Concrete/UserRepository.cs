using Task.Abstract;
using Task.Models;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Security.Claims;

namespace Task.DataAccess.Concrete
{
    public class UserRepository : EntityRepository<User>, IUserRepository
    {
        private readonly IConfiguration configuration;

        public UserRepository(ProductCustomerContext productCustomerContext, IConfiguration configuration) : base(productCustomerContext)
        {
            this.configuration = configuration;
        }
        public User Add(UserRegister request)
        {
            var password = BCrypt.Net.BCrypt.HashPassword(request.Password);

            var user = new User()
            {
                Name = request.Name,
                Password = password,
                Mail = request.Mail,
                Msisdn = request.Msisdn,
                Roles = request.Roles
            };
            productCustomerContext.Users.Add(user);
            productCustomerContext.SaveChanges();

            string token = CreateToken(user);
            user.Token = token;
            productCustomerContext.SaveChanges();

            return user;
        }

        public User GetUser(UserLogin request)
        {
            var user = productCustomerContext.Users.FirstOrDefault(u => u.Name == request.Name);

            if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.Password))
            {
                return null;
            }

            return user;
        }
        public string CreateToken(User user)
        {
            List<Claim> claims = new List<Claim>{
            new Claim(ClaimTypes.Name, user.Name),
            new Claim(ClaimTypes.Role, user.Roles)
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

        public void RoleAssignment(int Id, string role)
        {
            var user = productCustomerContext.Users.FirstOrDefault(u => u.Id == Id);

            if (user != null)
            {
                user.Roles = role;
                productCustomerContext.SaveChanges();
            }
        }
    }
}