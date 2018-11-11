using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DM.Models.ViewModels;
using DM.Models.Wrappers;

namespace DM.Logic.Interfaces
{
    public interface IFavouritesService
    {
        Task<Guid> AddToFavouritesAsync(FavouriteCreationVM favouriteCreation);
        Task<IndexedResult<IEnumerable<FavouriteVM>>> GetFavouriteMealsAsync(Guid userId, IndexedResult<FavouriteVM> lastReturned, int takeAmount = 10);
        Task<bool> RemoveFromFavouritesAsync(Guid userId, Guid mealId);
    }
}