using Microsoft.AspNetCore.Mvc;
using TheMemoryGameBackend.Data;
using TheMemoryGameBackend.Models;

namespace TheMemoryGameBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;

        public AuthController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpPost("register")]
        public IActionResult Register([FromBody] RegisterRequest request)
        {
            if (string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.Password))
            {
                return BadRequest(new Response<object>
                {
                    Success = false,
                    Message = "邮箱和密码不能为空",
                    Code = 400 // HTTP 400 Bad Request
                });
            }

            // 检查邮箱是否已注册
            if (_dbContext.Users.Any(u => u.Email == request.Email))
            {
                return Conflict(new Response<object>
                {
                    Success = false,
                    Message = "邮箱已被注册",
                    Code = 409 // HTTP 409 Conflict
                });
            }

            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(request.Password);

            // 添加用户到数据库
            var user = new User
            {
                Email = request.Email,
                Password = request.Password // 实际项目中请加密密码
            };
            _dbContext.Users.Add(user);
            _dbContext.SaveChanges();

            return Ok(new Response<object>
            {
                Success = true,
                Message = "注册成功",
                Code = 200 // HTTP 200 OK
            });
        }
    }
}
