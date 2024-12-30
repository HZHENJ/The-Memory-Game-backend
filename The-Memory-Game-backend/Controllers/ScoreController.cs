
using Microsoft.AspNetCore.Mvc;
using TheMemoryGameBackend.Data;
using TheMemoryGameBackend.Models;

namespace TheMemoryGameBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ScoreController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;

        public ScoreController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpPost("submit")]
        public IActionResult SubmitScore([FromBody] ScoreRequest request)
        {
            var user = _dbContext.Users.Find(request.UserId);
            if (user == null)   // 用户不存在
            {
                return BadRequest(new Response<object>
                {
                    Success = false,
                    Message = "User not found",
                    Code = 404
                });
            }

            var score = new Score
            {
                User = user,
                ElapsedTime = request.ElapsedTime
            };
            try {
                _dbContext.Scores.Add(score);
                _dbContext.SaveChanges();
            } catch (Exception e) {
                return BadRequest(new Response<object>
                {
                    Success = false,
                    Message = e.Message,
                    Code = 400
                });
            }

            return Ok(new Response<object>{
                Success = true,
                Message = "Score submitted",
                Code = 200
            });
        }
    }
}