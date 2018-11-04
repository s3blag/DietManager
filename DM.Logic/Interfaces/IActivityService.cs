using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DM.Models.ViewModels;
using DM.Models.Wrappers;

namespace DM.Logic.Interfaces
{
    public interface IActivityService
    {
        Task<IndexedResult<IList<UserActivityVM>>> GetUsersActivitiesFeedAsync(IEnumerable<Guid> userIds, IndexedResult<UserActivityVM> lastReturned, int takeAmount = 10);
        Task LogNewAchievementReachedAsync(Guid userId, Guid userAchievementId);
        Task LogNewMealAddedAsync(Guid userId, Guid mealId);
        Task LogNewMealIngredientAddedAsync(Guid userId, Guid mealIngredientId);
        Task LogNewFavouriteMealAddedAsync(Guid userId, Guid favouriteId);
    }
}