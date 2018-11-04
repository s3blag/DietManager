using AutoMapper;
using DM.Database;
using DM.Logic.Interfaces;
using DM.Models.Enums;
using DM.Models.Exceptions;
using DM.Models.Models;
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

        public async Task<IndexedResult<IList<UserActivityVM>>> GetUsersActivitiesFeedAsync(
            IEnumerable<Guid> userIds,
            IndexedResult<UserActivityVM> lastReturned,
            int takeAmount = Constants.DEFAULT_DB_TAKE_VALUE)
        {
            if (lastReturned != null && lastReturned.IsLast)
            {
                return null;
            }

            var usersActivities = await _activityRepository.GetUsersActivitiesAsync(userIds, lastReturned.Index, takeAmount);

            return new IndexedResult<IList<UserActivityVM>>()
            {
                Result = _mapper.Map<IList<UserActivityVM>>(usersActivities),
                Index = lastReturned?.Index ?? 0 + usersActivities.Count,
                IsLast = usersActivities.Count != takeAmount
            };
        }

        public async Task LogNewMealAddedAsync(Guid userId, Guid mealId)
        {
            var dbActivity = _mapper.Map<UserActivity>(new ActivityCreation() { UserId = userId, ContentId = mealId, ActivityType = ActivityType.NewMealAdded });

            await AddActivityAsync(dbActivity);
        }

        public async Task LogNewMealIngredientAddedAsync(Guid userId, Guid mealIngredientId)
        {
            var dbActivity = _mapper.Map<UserActivity>(new ActivityCreation() { UserId = userId, ContentId = mealIngredientId, ActivityType = ActivityType.NewMealIngredientAdded });

            await AddActivityAsync(dbActivity);
        }

        public async Task LogNewAchievementReachedAsync(Guid userId, Guid userAchievementId)
        {
            var dbActivity = _mapper.Map<UserActivity>(new ActivityCreation() { UserId = userId, ContentId = userAchievementId, ActivityType = ActivityType.AchievementReached });

            await AddActivityAsync(dbActivity);
        }

        private async Task AddActivityAsync(UserActivity model)
        {
            if (!await _activityRepository.AddAsync(model))
            {
                throw new DataAccessException($"Adding new activity failed for model: {JsonConvert.SerializeObject(model)}");
            }
        }

        public async Task LogNewFavouriteMealAddedAsync(Guid userId, Guid favouriteId)
        {
            var dbActivity = _mapper.Map<UserActivity>(new ActivityCreation() { UserId = userId, ContentId = favouriteId, ActivityType = ActivityType.AddToFavourites });

            await AddActivityAsync(dbActivity);
        }
    }
}
