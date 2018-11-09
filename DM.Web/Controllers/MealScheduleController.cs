﻿using System;
using System.Linq;
using System.Threading.Tasks;
using DM.Logic.Interfaces;
using DM.Models.ViewModels;
using DM.Web.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace DM.Web.Controllers
{
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
        public async Task<IActionResult> GetMealIngredient(DateTimeOffset weekStartDate)
        {
            if (weekStartDate == default)
            {
                return NotFound();
            }

            var userId = Guid.Empty;

            var mealSchedule = await _mealScheduleService.GetUpcomingMealSchedule(userId, weekStartDate);

            if (mealSchedule == null || !mealSchedule.Any())
            {
                return NotFound();
            }

            return Ok(mealSchedule.ParseEnumToLower());
        }

        [HttpPost("entry")]
        public async Task<IActionResult> AddMealScheduleEntry([FromBody]MealScheduleEntryCreationVM mealScheduleCreation)
        {
            if (!ModelState.IsValid)
            {
                return NotFound();
            }

            var userId = Guid.Empty;

            var entryId = await _mealScheduleService.AddMealScheduleEntry(userId, mealScheduleCreation);

            return Ok(entryId);
        }
    }
}
