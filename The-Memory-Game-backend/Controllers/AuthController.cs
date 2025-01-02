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

        private string GenerateUniqueUsername(string email){
            // Generate a unique username based on the email and timestamp
            var emailPrefix = email.Split('@')[0];
            var timestamp = DateTime.Now.ToString("yyyyMMddHHmmss");
            var username = emailPrefix + timestamp;

            // Verify uniqueness and avoid conflicts
            int suffix = 1;
            while (_dbContext.Users.Any(u => u.Username == username))
            {
                username = $"{emailPrefix}_{timestamp}_{suffix}";
                suffix++;
            }
            return username;
        }

        [HttpPost("register")]
        public IActionResult Register([FromBody] UserRequest request)
        {
            if (string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.Password))
            {
                return BadRequest(new Response<object>
                {
                    Success = false,
                    Message = "Email and password cannot be empty",
                    Code = 400 // HTTP 400 Bad Request
                });
            }

            // 检查邮箱是否已注册
            if (_dbContext.Users.Any(u => u.Email == request.Email))
            {
                return Conflict(new Response<object>
                {
                    Success = false,
                    Message = "Email has been registered",
                    Code = 409 // HTTP 409 Conflict
                });
            }

            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(request.Password);
            var username = GenerateUniqueUsername(request.Email);

            // Add user data into the database
            var user = new User
            {
                Email = request.Email,
                Password = hashedPassword,
                Username = username,
                Type = 0
            };
            _dbContext.Users.Add(user);
            _dbContext.SaveChanges();

            return Ok(new Response<object>
            {
                Success = true,
                Message = "Successfully registered",
                Code = 200, // HTTP 200 OK
                Data = new { 
                    user.Id,
                    user.Email,
                    user.Username,
                    user.Type  
                 }
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

            if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.Password)) {
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
                    user.Email,
                    user.Username,
                    user.Type          
                }
            });
        }
    
        [HttpPost("payment")]
        public IActionResult Payment([FromBody] UserRequestWithId request){
            var user = _dbContext.Users.FirstOrDefault(u => u.Id == request.Id);
            if (user != null){
                if (user.Type == 1){
                    user.Type = 0;
                    _dbContext.SaveChanges();
                    return Ok(new Response<object>{
                        Success = true,
                        Message = "Cancel successful",
                        Code = 200
                    });
                } else {
                    // Payment logic
                    user.Type = 1;
                    _dbContext.SaveChanges();
                    return Ok(new Response<object>{
                        Success = true,
                        Message = "Payment successful",
                        Code = 200
                    });
                }
            } else {
                return NotFound(new Response<object>{
                    Success = false,
                    Message = "User not found",
                    Code = 404
                });
            }
        }

        [HttpGet("getUserDetails")]
        public IActionResult GetUserDetails([FromQuery] int userId){
            var user = _dbContext.Users.FirstOrDefault(u => u.Id == userId);
            if (user != null){
                return Ok(new Response<object>{
                    Success = true,
                    Message = "User details retrieved",
                    Code = 200,
                    Data = new {
                        user.Id,
                        user.Email,
                        user.Username,
                        user.Type
                    }
                });
            } else {
                return NotFound(new Response<object>{
                    Success = false,
                    Message = "User not found",
                    Code = 404
                });
            }
        }

    
    }
}
