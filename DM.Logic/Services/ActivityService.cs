using AutoMapper;
using DM.Database;
using DM.Logic.Interfaces;
using DM.Models.Exceptions;
using DM.Models.ViewModels;
using DM.Models.Wrappers;
using DM.Repositories.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DM.Logic.Services
{
    public class ActivityService : IActivityService
    {
        private readonly IActivityRepository _activityRepository;
        private readonly IMapper _mapper;

        public ActivityService(IActivityRepository activityRepository, IMapper mapper)
        {
            _activityRepository = activityRepository;
            _mapper = mapper;
        }

        public async Task<IndexedResult<ICollection<UserActivityVM>>> GetUserActivitiesAsync(
            Guid userId,
            IndexedResult<UserActivityVM> lastReturned,
            int takeAmount = Constants.DEFAULT_DB_TAKE_VALUE)
        {
            var usersActivities = await _activityRepository.GetUserActivitiesAsync(userId, lastReturned?.Index ?? 0, takeAmount);

            return new IndexedResult<ICollection<UserActivityVM>>()
            {
                Result = _mapper.Map<ICollection<UserActivityVM>>(usersActivities),
                Index = lastReturned?.Index ?? 0 + usersActivities.Count,
                IsLast = usersActivities.Count != takeAmount
            };
        }

        public async Task LogNewMealAddedAsync(Guid userId, Guid mealId) => 
            await AddActivityAsync(new UserActivity() { UserId = userId, MealId = mealId });

        public async Task LogNewMealIngredientAddedAsync(Guid userId, Guid mealIngredientId) => 
            await AddActivityAsync(new UserActivity() { UserId = userId, MealIngredientId = mealIngredientId });

        public async Task LogNewAchievementReachedAsync(Guid userId, Guid achievementId) =>
            await AddActivityAsync(new UserActivity() { UserId = userId, AchievementId = achievementId });

        public async Task LogNewFavouriteMealAddedAsync(Guid userId, Guid mealId) =>
            await AddActivityAsync(new UserActivity() { UserId = userId, FavouriteId = mealId });

        private async Task AddActivityAsync(UserActivity model)
        {
            if (!await _activityRepository.AddAsync(model))
            {
                throw new DataAccessException($"Adding new activity failed for model: {JsonConvert.SerializeObject(model)}");
            }
        }
    }
}