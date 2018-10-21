using System;
using System.Threading.Tasks;
using DM.Logic.Interfaces;
using DM.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace DM.Web.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class MealIngredientController : Controller
    {
        private readonly IMealIngredientService _mealIngredientService;

        public MealIngredientController(IMealIngredientService mealIngredientService)
        {
            _mealIngredientService = mealIngredientService;
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

            var mealIngredient = await _mealIngredientService.AddMealIngredientAsync(mealIngredientCreationVM);

            return Ok(mealIngredient);
        }


    }
}