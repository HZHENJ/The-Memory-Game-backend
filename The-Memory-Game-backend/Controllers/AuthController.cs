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
        public IActionResult Register([FromBody] UserRequest request)
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
                Password = hashedPassword
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

        [HttpPost("login")]
        public IActionResult Login([FromBody] UserRequest request)
        {
            // Check if the requested data is valid
            if (string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.Password)) {
                return BadRequest(new Response<object> {
                    Success = false,
                    Message = "Email and password cannot be empty",
                    Code = 400 // HTTP 400 Bad Request
                });
            }

            // Find users
            var user = _dbContext.Users.FirstOrDefault(u => u.Email == request.Email);
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(request.Password);

            if (user == null || hashedPassword != user.Password) {
                return Unauthorized(new Response<object> {
                    Success = false,
                    Message = "Email or password is incorrect",
                    Code = 401 // HTTP 401 Unauthorized
                });
            }

            return Ok(new Response<object>
            {
                Success = true,
                Message = "Login successful",
                Code = 200, // HTTP 200 OK
                Data = new {
                    user.Id,
                    user.Email
                }
            });
        }
    }

}
