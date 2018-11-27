using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DM.Logic.Interfaces;
using DM.Models.ViewModels;
using DM.Models.Wrappers;
using DM.Web.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace DM.Web.Controllers
{
    //TODO Model State Validator

    [Produces("application/json")]
    [Route("api/[controller]")]
    public class MealController : Controller
    {
        private readonly IMealService _mealService;
        private readonly IMealIngredientService _mealIngredientService;
        private readonly ISearchService _searchService;

        public MealController(IMealService mealService, IMealIngredientService mealIngredientService,
            ISearchService searchService)
        {
            _mealService = mealService;
            _mealIngredientService = mealIngredientService;
            _searchService = searchService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetMeal(Guid id)
        {
            if (id == Guid.Empty)
            {
                return NotFound();
            }

            var meal = await _mealService.GetMealByIdAsync(id);

            if (meal == null)
            {
                return NotFound();
            }

            return Ok(meal);
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddMeal([FromBody] MealCreationVM mealCreationVM)
        {
            if (!ModelState.IsValid)
            {
                return NotFound("ViewModelIsInvalid");
            }

            var userId = Guid.Empty;

            var mealId = await _mealService.AddMealAsync(mealCreationVM, userId);

            if (mealId == Guid.Empty)
            {
                return NotFound();
            }

            return Ok(mealId);
        }

        [HttpGet("{id}/meal-ingredients")]
        public async Task<IActionResult> GetMealIngredients(Guid mealId)
        {
            var mealIngredients = await _mealIngredientService.GetMealIngredientsForMealAsync(mealId);

            if (mealIngredients.Any())
            {
                return NotFound();
            }

            return Ok(mealIngredients);
        }

        [HttpPost("meal-previews")]
        public async Task<IActionResult> GetMealPreviewsForUser([FromBody]IndexedResult<MealPreviewVM> lastReturned)
        {
            if (lastReturned != null && lastReturned.IsLast)
            {
                return NotFound("Invalid arguments");
            }

            var userId = Guid.Empty;

            var result = await _mealService.GetUsersMealsPreviewsAsync(userId, lastReturned);

            return Ok(result);
        }

        
        [HttpPost("search")]
        public async Task<IActionResult> SearchMeals([FromBody]IndexedResult<MealSearchVM> lastReturned)
        {
            if (lastReturned != null && lastReturned.IsLast)
            {
                return NotFound("Invalid arguments");
            }

            var userId = Guid.Empty;

            var result = await _searchService.SearchMealsAsync(userId, lastReturned);

            return Ok(result);
        }
    }
}