using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DM.Database;
using DM.Models.ViewModels;

namespace DM.Logic.Interfaces
{
    public interface IAchievementService
    {
        Task<UserAchievementVM> CheckForConsequentScheduleUpdatesAsync(Guid userId);

        Task CheckForNumberOfFavouriteMarksAsync(Guid mealId);

        Task<UserAchievementVM> CheckForNumberOfFriendsAsync(Guid userId);

        Task<UserAchievementVM> CheckForNumberOfMealAdditionsByUserAsync(User user);

        Task<UserAchievementVM> CheckForNumberOfMealIngredientAdditionsByUserAsync(User user);

        Task CheckForNumberOfMealUsesAsync(Guid userId, Guid mealId);

        Task<UserAchievementVM> CheckForUserAnniversaryAsync(User userBeforeLastLoginUpdate);

        Task<IEnumerable<UserAchievementVM>> GetUsersAchievements(Guid userId);

        Task<bool> MarkAchievementsAsReadAsync(IEnumerable<Guid> userAchievementIds, Guid userId);

        Task<IEnumerable<AchievementVM>> GetAllAchievementsAsync();

    }
}