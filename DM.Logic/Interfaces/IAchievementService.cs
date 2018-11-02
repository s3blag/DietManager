using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DM.Database;
using DM.Models.ViewModels;

namespace DM.Logic.Interfaces
{
    public interface IAchievementService
    {
        Task<UserAchievementVM> CheckForConsequentScheduleUpdatesAchievementAsync(Guid userId);
        Task CheckForNumberOfFavouriteMarksAchievementAsync(Meal mealAfterUpdate);
        Task<UserAchievementVM> CheckForNumberOfFriendsAchievementAsync(Guid userId);
        Task<UserAchievementVM> CheckForNumberOfMealAdditionsByUserAsync(Guid userId);
        Task<UserAchievementVM> CheckForNumberOfMealIngredientAdditionsByUserAchievementAsync(Guid userId);
        Task CheckForNumberOfMealUsesAchievementAsync(Meal mealAfterScheduleUpdate);
        Task<UserAchievementVM> CheckForUserAnniversaryAchievementAsync(User userBeforeLastLoginUpdate);
        Task<IEnumerable<UserAchievementVM>> GetUsersAchievements(Guid userId);
        Task<bool> MarkAchievementsAsReadAsync(IEnumerable<Guid> userAchievementIds, Guid userId);
    }
}