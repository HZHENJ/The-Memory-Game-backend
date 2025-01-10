using Microsoft.AspNetCore.Mvc;
using TheMemoryGameBackend.Data;
using TheMemoryGameBackend.Models;

namespace TheMemoryGameBackend.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class LeaderBoardController : ControllerBase
	{
		private readonly ApplicationDbContext _dbContext;

		public LeaderBoardController(ApplicationDbContext dbContext)
		{
			_dbContext = dbContext;
		}

		// 获取排行榜
		[HttpGet("getScores")]
		public IActionResult GetScores()
		{
			// 获取排名前十的用户及其分数
			var scores = _dbContext.Scores
				.OrderBy(s => s.ElapsedTime)
				.Take(10)
				.Select(s => new
				{
					Username = s.User.Username,
					ElapsedTime = s.ElapsedTime
				});

			return Ok(new Response<object>
			{
				Success = true,
				Message = "Scores retrieved",
				Code = 200,
				Data = scores
			});
		}
	}

}