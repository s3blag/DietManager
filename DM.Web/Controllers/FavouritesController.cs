using DM.Logic.Interfaces;
using DM.Models.ViewModels;
using DM.Models.Wrappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace DM.Web.Controllers
{
    [Authorize]
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class FavouritesController : Controller
    {
        private readonly IFavouritesService _favouritesService;

        public FavouritesController(IFavouritesService favouritesService)
        {
            _favouritesService = favouritesService;
        }

        [HttpPost("get")]
        public async Task<IActionResult> GetFavouriteMeals(IndexedResult<MealPreviewVM> lastReturned)
        {
            if (lastReturned != null && lastReturned.IsLast)
            {
                return NotFound("Invalid arguments");
            }

            var userId = new Guid(User.Identity.Name);

            var favourites = await _favouritesService.GetFavouriteMealsAsync(userId, lastReturned);

            if (favourites == null)
            {
                return NotFound();
            }

            return Ok(favourites);
        }

        [HttpPost]
        public async Task<IActionResult> AddToFavourites([FromBody]FavouriteCreationVM favouriteCreation)
        {
            if (favouriteCreation.MealId == null || favouriteCreation.MealId == Guid.Empty)
            {
                return NotFound();
            }

            var userId = new Guid(User.Identity.Name);
            favouriteCreation.UserId = userId;

            var favouriteId = await _favouritesService.AddToFavouritesAsync(favouriteCreation);

            return Ok(favouriteId);
        }

        [HttpDelete("{mealId}")]
        public async Task<IActionResult> DeleteFromFavourites(Guid mealId)
        {
            if (mealId == Guid.Empty)
            {
                return NotFound();
            }

            var userId = new Guid(User.Identity.Name);

            bool deleted = await _favouritesService.RemoveFromFavouritesAsync(userId, mealId);

            if (deleted)
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