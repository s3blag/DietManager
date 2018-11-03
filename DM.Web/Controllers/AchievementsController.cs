using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DM.Logic.Interfaces;
using DM.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace DM.Web.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class AchievementsController : Controller
    {
        private readonly IAchievementService _achievementService;

        public AchievementsController(IAchievementService achievementService)
        {
            _achievementService = achievementService;
        }

        [HttpGet("")]
        public async Task<IActionResult> GetAllAchievements()
        {
            var result = await _achievementService.GetAllAchievementsAsync();

            if (result.Any())
            {
                return Ok(result);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpGet("my-achievements")]
        public async Task<IActionResult> GetUserAchievements()
        {
            var userId = Guid.Empty;

            var result = await _achievementService.GetUsersAchievements(userId);

            return Ok(result);
        }

        [HttpPost("my-achievements/mark-as-read")]
        public async Task<IActionResult> MarkAsRead([FromBody]IEnumerable<UserAchievementVM> userAchievements)
        {
            if (userAchievements == null || !userAchievements.Any() || userAchievements.Any(ua => ua.Seen == true))
            {
                return NotFound();
            }

            var userId = Guid.Empty;

            var result = await _achievementService.MarkAchievementsAsReadAsync(userAchievements.Select(ua => ua.Id).ToList(), userId);

            return Ok(result);
        }
    }
}