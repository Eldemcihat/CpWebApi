using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.IdentityModel.Tokens;
using Task.Business.Abstract;
using Task.Models;

namespace Task.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService userService;
        private readonly IMemoryCache memoryCache;
        private readonly IConfiguration configuration;
        private const string CachePrefix = "User_";
        public UserController(IUserService userService, IMemoryCache memoryCache, IConfiguration configuration)
        {
            this.userService = userService;
            this.memoryCache = memoryCache;
            this.configuration = configuration;
        }


        private string GenerateToken(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.Role, user.Roles),
                new Claim(ClaimTypes.Email,user.Mail),
                new Claim(ClaimTypes.MobilePhone,user.Msisdn)
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
        //[Authorize(Roles = "Admin")]
        [HttpPost("register")]
        public ActionResult<User> Register(UserRegister request)
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

            var result = userService.Add(request);
            var cacheKey = $"{CachePrefix}GetById_{result.Id}";
            memoryCache.Set(cacheKey, result, TimeSpan.FromMinutes(10));

            string token = GenerateToken(result);

            var response = new { Token = token };
            return Ok(response);
        }

        [HttpPost("login")]
        public ActionResult<User> login(UserLogin request)
        {
            var cacheKey = $"{CachePrefix}GetUser_{request.Name}_{request.Password}";
            var result = memoryCache.GetOrCreate(cacheKey, entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10);
                return userService.GetUser(request);
            });

            string token = GenerateToken(result);

            var response = new { Token = token };
            return Ok(response);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("GetAll")]
        public IEnumerable<User> Get()
        {

            var cacheKey = $"{CachePrefix}GetAll";
            var result = memoryCache.GetOrCreate(cacheKey, entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10);
                var users = userService.GetAll();
                return users.Select(u => new User { Mail = u.Mail, Msisdn = u.Msisdn, Name = u.Name, Roles = u.Roles, Password = u.Password, Id = u.Id }).ToList();
            });

            return result;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("GetById{id}")]
        public User Get(int id)
        {
            var cacheKey = $"{CachePrefix}GetById_{id}";
            var result = memoryCache.GetOrCreate(cacheKey, entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10);
                return userService.GetById(id);
            });

            return result;
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("Update")]
        public User Update([FromBody] User entity)
        {
            var cacheKey = $"{CachePrefix}Update_{entity.Id}";
            var result = userService.Update(entity);
            memoryCache.Set(cacheKey, result, TimeSpan.FromMinutes(10));
            return result;
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("Delete{id}")]
        public void Delete(int id)
        {
            var cacheKey = $"{CachePrefix}Delete_{id}";
            userService.Delete(id);
            memoryCache.Remove(cacheKey);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("roleassignment")]
        public void RoleAssignment(int Id, string role)
        {
            userService.RoleAssignment(Id, role);
            var cacheKey = $"{CachePrefix}GetById_{Id}";
            memoryCache.Remove(cacheKey);
        }
    }
}