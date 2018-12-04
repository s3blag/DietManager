﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DM.Database;
using DM.Models.ViewModels;

namespace DM.Logic.Interfaces
{
    public interface IAchievementService
    {
        Task<GroupedUserAchievementsVM> GetUserAchievementsAsync(Guid userId);
        Task<bool> MarkAchievementsAsReadAsync(IEnumerable<Guid> userAchievementIds, Guid userId);
        Task<IEnumerable<AchievementVM>> GetAllAchievementsAsync();

        Task CheckForConsequentScheduleUpdatesAsync(Guid userId);
        Task CheckForNumberOfFavouriteMarksAsync(Guid mealId);
        Task CheckForNumberOfFriendsAsync(Guid userId);
        Task CheckForNumberOfMealAdditionsByUserAsync(User user);
        Task CheckForNumberOfMealIngredientAdditionsByUserAsync(User user);
        Task CheckForNumberOfMealUsesAsync(Guid mealId);
        Task CheckForUserAnniversaryAsync(User userBeforeLastLoginUpdate);
    }
}