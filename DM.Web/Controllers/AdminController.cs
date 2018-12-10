using DM.Logic.Interfaces;
using DM.Models.ViewModels;
using DM.Models.Wrappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DM.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    [Produces("application/json")]
    [Route("api")]
    public class AdminController : Controller
    {
        private readonly IAdminService _adminService;

        public AdminController(IAdminService adminService)
        {
            _adminService = adminService;
        }

        [HttpPost("user/activities")]
        public async Task<IActionResult> GetUsersActivities([FromBody]IndexedResult<UserActivityVM> lastReturned)
        {
            if (lastReturned != null && lastReturned.IsLast)
            {
                return NotFound("Invalid arguments");
            }

            var activities = await _adminService.GetUsersActivitiesAsync(lastReturned);

            return Ok(activities);
        }

        [HttpDelete("user/{userId}")]
        public async Task<IActionResult> DeleteUserAsync(Guid userId)
        {
            if (userId == Guid.Empty)
            {
                return NotFound("Invalid arguments");
            }

            bool deleted = await _adminService.DeleteUserAsync(userId);

            if (!deleted)
            {
                return BadRequest();
            }

            return Ok();
        }

        [HttpDelete("meal/{mealId}")]
        public async Task<IActionResult> DeleteMealAsync(Guid mealId)
        {
            if (mealId == Guid.Empty)
            {
                return NotFound("Invalid arguments");
            }

            bool deleted = await _adminService.DeleteMealAsync(mealId);

            if (!deleted)
            {
                return BadRequest();
            }

            return Ok();
        }

        [HttpDelete("meal-ingredient/{mealIngredientId}")]
        public async Task<IActionResult> DeleteMealIngredientAsync(Guid mealIngredientId)
        {
            if (mealIngredientId == Guid.Empty)
            {
                return NotFound("Invalid arguments");
            }

            bool deleted = await _adminService.DeleteMealIngredientAsync(mealIngredientId);

            if (!deleted)
            {
                return BadRequest();
            }

            return Ok();
        }

        [HttpPost("user/activities/mark-as-seen")]
        public async Task<IActionResult> MarkActivitiesAsSeen([FromBody]IEnumerable<int> activitiesIds)
        {
            if (activitiesIds == null || !activitiesIds.Any())
            {
                return NotFound("Invalid arguments");
            }

            bool markedAsSeen = await _adminService.MarkActivitiesAsSeenAsync(activitiesIds);

            if (!markedAsSeen)
            {
                return BadRequest();
            }

            return Ok();
        }

    }
}