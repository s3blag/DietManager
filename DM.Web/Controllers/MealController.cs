using System;
using System.Linq;
using System.Threading.Tasks;
using DM.Logic.Interfaces;
using DM.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace DM.Web.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class MealController : Controller
    {
        private readonly IMealService _mealService;
        private readonly IMealIngredientService _mealIngredientService;

        public MealController(IMealService mealService, IMealIngredientService mealIngredientService)
        {
            _mealService = mealService;
            _mealIngredientService = mealIngredientService;
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

        [HttpPost("add",Name ="addMeal")]
        public async Task<IActionResult> AddMeal([FromBody] MealCreationVM mealCreationVM)
        {
            if (!ModelState.IsValid)
            {
                return NotFound("ViewModelIsInvalid");
            }

            var mealId = await _mealService.AddMealAsync(mealCreationVM);

            if (mealId == Guid.Empty)
            {
                return NotFound();
            }

            return Ok(mealId);
        }

        [HttpGet("{id}/MealIngredients", Name = "GetMealIngredientsForMeal")]
        public async Task<IActionResult> GetMealIngredients(Guid mealId)
        {
            var mealIngredients = await _mealIngredientService.GetMealIngredientsForMealAsync(mealId);

            if (mealIngredients.Any())
            {
                return NotFound();
            }

            return Ok(mealIngredients);
        }
    }
}