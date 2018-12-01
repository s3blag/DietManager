using System;
using System.Linq;
using System.Threading.Tasks;
using DM.Logic.Interfaces;
using DM.Models.ViewModels;
using DM.Models.Wrappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DM.Web.Controllers
{
    [Authorize]
    [Produces("application/json")]
    [Route("api/meal-ingredient")]
    public class MealIngredientController : Controller
    {
        private readonly IMealIngredientService _mealIngredientService;
        private readonly ISearchService _searchService;

        public MealIngredientController(IMealIngredientService mealIngredientService, ISearchService searchService)
        {
            _mealIngredientService = mealIngredientService;
            _searchService = searchService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetMealIngredient(Guid id)
        {
            var meal = await _mealIngredientService.GetMealIngredientAsync(id);

            if (meal == null)
            {
                return NotFound();
            }

            return Ok(meal);
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddMealIngredient([FromBody]MealIngredientCreationVM mealIngredientCreationVM)
        {
            if (!ModelState.IsValid)
            {
                return NotFound();
            }

            var userId = new Guid(User.Identity.Name);

            var mealIngredient = await _mealIngredientService.AddMealIngredientAsync(userId, mealIngredientCreationVM);

            return Ok(mealIngredient);
        }

        [HttpPost("search")]
        public async Task<IActionResult> SearchMealIngredients([FromBody]IndexedResult<MealIngredientSearchVM> lastReturned)
        {
            if (lastReturned != null && lastReturned.IsLast)
            {
                return NotFound("Invalid arguments");
            }

            var result = await _searchService.SearchMealIngredientsAsync(lastReturned);

            return Ok(result);
        }

    }
}