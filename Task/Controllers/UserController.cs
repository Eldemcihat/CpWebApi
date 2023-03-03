using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
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
        private const string CachePrefix = "User_";
        public UserController(IUserService userService, IMemoryCache memoryCache)
        {
            this.userService = userService;
            this.memoryCache = memoryCache;
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("register")]
        public ActionResult<User> Register(UserRegister request)
        {
            var result = userService.Add(request);
            var cacheKey = $"{CachePrefix}GetById_{result.Id}";
            memoryCache.Set(cacheKey, result, TimeSpan.FromMinutes(10));
            return result;
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

            return result;
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
                return users.Select(u => new User { Mail = u.Mail, Msisdn = u.Msisdn, Name = u.Name, Token = u.Token, Roles = u.Roles, Password = u.Password, Id = u.Id }).ToList();
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