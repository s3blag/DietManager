using System;
using System.Linq;
using System.Threading.Tasks;
using DM.Logic.Interfaces;
using DM.Models.ViewModels;
using DM.Web.Attributes;
using DM.Web.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DM.Web.Controllers
{
    [Authorize]
    [Produces("application/json")]
    [Route("api/meal-schedule")]
    public class MealScheduleController : Controller
    {
        private readonly IMealScheduleService _mealScheduleService;

        public MealScheduleController(IMealScheduleService mealScheduleService)
        {
            _mealScheduleService = mealScheduleService;
        }

        [HttpGet("week/{weekStartDate}")]
            [ModelStateValidator]
        public async Task<IActionResult> GetMealScheduleEntries(DateTimeOffset weekStartDate)
        {
            if (weekStartDate == default)
            {
                return NotFound();
            }

            var userId = new Guid(User.Identity.Name);

            var mealSchedule = await _mealScheduleService.GetUpcomingMealSchedule(userId, weekStartDate);

            if (mealSchedule == null || !mealSchedule.Any())
            {
                return NotFound();
            }

            return Ok(mealSchedule.ParseEnumToLower());
        }

        [HttpPost("entry")]
        [ModelStateValidator]
        public async Task<IActionResult> AddMealScheduleEntry([FromBody]MealScheduleEntryCreationVM mealScheduleCreation)
        {
            if (!ModelState.IsValid)
            {
                return NotFound();
            }

            var userId = new Guid(User.Identity.Name);

            var entryId = await _mealScheduleService.AddMealScheduleEntryAsync(userId, mealScheduleCreation);

            if (entryId == null)
            {
                return Unauthorized();
            } 

            return Ok(entryId);
        }

        [HttpDelete("entry/{scheduleEntryId}")]
        public async Task<IActionResult> DeleteMealScheduleEntry(Guid scheduleEntryId)
        {
            if (scheduleEntryId == Guid.Empty)
            {
                return NotFound();
            }

            var userId = new Guid(User.Identity.Name);

            bool deleted = await _mealScheduleService.DeleteMealScheduleEntryAsync(userId, scheduleEntryId);

            if (deleted)
            {
                return Ok();
            }
            else
            {
                return Unauthorized();
            }
        }

        [HttpPatch("entry")]
        [ModelStateValidator]
        public async Task<IActionResult> UpdateMealScheduleEntry([FromBody] MealScheduleEntryUpdateVM scheduleEntryUpdate)
        {
            var userId = new Guid(User.Identity.Name);

            bool updated = await _mealScheduleService.UpdateMealScheduleEntryAsync(userId, scheduleEntryUpdate);

            if (updated)
            {
                return Ok();
            }
            else
            {
                return Unauthorized();
            }
        }
    }
}
