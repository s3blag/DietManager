using System;
using System.Linq;
using System.Threading.Tasks;
using DM.Logic.Interfaces;
using DM.Models.ViewModels;
using DM.Models.Wrappers;
using DM.Web.Attributes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DM.Web.Controllers
{
    [Authorize]
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

        [HttpGet("{mealId}")]
        [ModelStateValidator]
        public async Task<IActionResult> GetMeal(Guid mealId)
        {
            if (mealId == Guid.Empty)
            {
                return NotFound();
            }

            var userId = new Guid(User.Identity.Name);

            var meal = await _mealService.GetMealByIdAsync(userId, mealId);

            if (meal == null)
            {
                return NotFound();
            }

            return Ok(meal);
        }

        [HttpPost("add")]
        [ModelStateValidator]
        public async Task<IActionResult> AddMeal([FromBody] MealCreationVM mealCreationVM)
        {
            var userId = new Guid(User.Identity.Name);

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

            var userId = new Guid(User.Identity.Name);

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

            var userId = new Guid(User.Identity.Name);

            var result = await _searchService.SearchMealsAsync(userId, lastReturned);

            return Ok(result);
        }
    }
}