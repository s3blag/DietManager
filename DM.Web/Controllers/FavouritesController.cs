using DM.Logic.Interfaces;
using DM.Models.ViewModels;
using DM.Models.Wrappers;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace DM.Web.Controllers
{
    [Produces("application/json")]
    [Route("api/Favourites")]
    public class FavouritesController : Controller
    {
        private readonly IFavouritesService _favouritesService;

        public FavouritesController(IFavouritesService favouritesService)
        {
            _favouritesService = favouritesService;
        }

        [HttpPost("get")]
        public async Task<IActionResult> GetFavouriteMeals(IndexedResult<FavouriteVM> lastReturned)
        {
            if (lastReturned != null && lastReturned.IsLast)
            {
                return NotFound("Invalid arguments");
            }

            var userId = Guid.Empty;

            var favourites = await _favouritesService.GetFavouriteMealsAsync(userId, lastReturned);

            if (favourites == null)
            {
                return NotFound();
            }

            return Ok(favourites);
        }

        [HttpPost]
        public async Task<IActionResult> AddToFavourites([FromBody]Guid mealId)
        {
            if (mealId == Guid.Empty)
            {
                return NotFound();
            }

            var userId = Guid.Empty;

            var favouriteId = await _favouritesService.AddToFavouritesAsync(new FavouriteCreationVM() { MealId = mealId, UserId = userId });

            return Ok(favouriteId);
        }

        [HttpDelete]
        public async Task<IActionResult> AddToFavourites([FromBody]FavouriteVM favouriteToDelete)
        {
            if (favouriteToDelete == null || favouriteToDelete.Id == Guid.Empty)
            {
                return NotFound();
            }

            await _favouritesService.RemoveFromFavouritesAsync(favouriteToDelete);

            return Ok();
        }
    }
}