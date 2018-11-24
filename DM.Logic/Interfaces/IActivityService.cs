using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DM.Models.ViewModels;
using DM.Models.Wrappers;

namespace DM.Logic.Interfaces
{
    public interface IActivityService
    {
        Task<IndexedResult<ICollection<UserActivityVM>>> GetUserActivitiesAsync(Guid userId, IndexedResult<UserActivityVM> lastReturned, int takeAmount = 10);
        Task LogNewAchievementReachedAsync(Guid userId, Guid achievementId);
        Task LogNewMealAddedAsync(Guid userId, Guid mealId);
        Task LogNewMealIngredientAddedAsync(Guid userId, Guid mealIngredientId);
        Task LogNewFavouriteMealAddedAsync(Guid userId, Guid mealId);
    }
}