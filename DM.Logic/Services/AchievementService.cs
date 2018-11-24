using AutoMapper;
using DM.Database;
using DM.Logic.Interfaces;
using DM.Models.Config;
using DM.Models.Enums;
using DM.Models.Exceptions;
using DM.Models.Extensions;
using DM.Models.Models;
using DM.Models.ViewModels;
using DM.Repositories.Interfaces;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DM.Logic.Services
{
    public class AchievementService : IAchievementService
    {
        private readonly AchievementsConfig achievementsConfig;
        private readonly IUserAchievementRepository _userAchievementRepository;
        private readonly IAchievementRepository _achievementRepository;
        private readonly IMealRepository _mealRepository;
        private readonly IFriendRepository _friendRepository;
        private readonly IMealScheduleRepository _mealScheduleRepository;
        private readonly IActivityService _activityService;
        private readonly IMapper _mapper;

        public AchievementService(
            IOptions<AchievementsConfig> options,
            IUserAchievementRepository userAchievementRepository,
            IAchievementRepository achievementRepository,
            IMealRepository mealRepository,
            IFavouriteRepository favouriteRepository,
            IMealIngredientRepository mealIngredientRepository,
            IFriendRepository friendRepository,
            IMealScheduleRepository mealScheduleRepository,
            IActivityService activityService,
            IMapper mapper)
        {
            achievementsConfig = options.Value;
            _userAchievementRepository = userAchievementRepository;
            _achievementRepository = achievementRepository;
            _mealRepository = mealRepository;
            _friendRepository = friendRepository;
            _mealScheduleRepository = mealScheduleRepository;
            _activityService = activityService;
            _mapper = mapper;
        }

        public async Task<IEnumerable<AchievementVM>> GetAllAchievementsAsync() => 
            _mapper.Map<IEnumerable<AchievementVM>>(await _achievementRepository.GetAllAsync());

        public async Task<IEnumerable<UserAchievementVM>> GetUsersAchievements(Guid userId) => 
            _mapper.Map<IEnumerable<UserAchievementVM>>(await _userAchievementRepository.GetUsersAchievementsAsync(userId));
        
        public async Task<bool> MarkAchievementsAsReadAsync(IEnumerable<Guid> userAchievementIds, Guid userId)
        {
            return await _userAchievementRepository.MarkAsReadAsync(userAchievementIds, userId);
        }

        #region Meal Achievements

        public async Task CheckForNumberOfMealUsesAsync(Guid userId, Guid mealId)
        {
            var getMealTask = _mealRepository.GetMealByIdAsync(mealId);

            var currentlyBestValueTask = _userAchievementRepository.GetUserAchievementMaxValueAsync(userId, Achievements.MealAchievement.NumberOfUses);

            await Task.WhenAll(getMealTask, currentlyBestValueTask);

            if (getMealTask.Result.NumberOfUses <= currentlyBestValueTask.Result)
            {
                return;
            }

            int[] achievementStages = achievementsConfig.MealAchievements[Achievements.MealAchievement.NumberOfUses];

            await AddAchievementIfNextStageReachedAsync(userId, achievementStages, getMealTask.Result.NumberOfUses, Achievements.MealAchievement.NumberOfUses);
        }

        public async Task CheckForNumberOfMealAdditionsByUserAsync(User user)
        {
            var currentlyBestValue = await _userAchievementRepository.GetUserAchievementMaxValueAsync(user.Id, Achievements.MealAchievement.AdditionsCountOver);

            if (user.CreatedMealsCount <= currentlyBestValue)
            {
                return;
            }

            int[] achievementStages = achievementsConfig.MealAchievements[Achievements.MealAchievement.AdditionsCountOver];

            await AddAchievementIfNextStageReachedAsync(user.Id, achievementStages, user.CreatedMealsCount, Achievements.MealAchievement.AdditionsCountOver);
        }

        public async Task CheckForNumberOfFavouriteMarksAsync(Guid mealId)
        {
            int[] achievementStages = achievementsConfig.MealAchievements[Achievements.MealAchievement.NumberOfFavouriteMarks];
      
            var meal =  await _mealRepository.GetMealByIdAsync(mealId);

            int currentlyBestValue = await _userAchievementRepository.GetUserAchievementMaxValueAsync(meal.CreatorId, Achievements.MealAchievement.NumberOfFavouriteMarks);

            if (meal.NumberOfFavouriteMarks <= currentlyBestValue)
            {
                return;
            }

            await AddAchievementIfNextStageReachedAsync(meal.CreatorId, achievementStages, meal.NumberOfFavouriteMarks, Achievements.MealAchievement.NumberOfFavouriteMarks);
        }

        #endregion

        #region MealIngredient Achievements

        public async Task CheckForNumberOfMealIngredientAdditionsByUserAsync(User user)
        {
            var currentlyBestValue = await _userAchievementRepository.GetUserAchievementMaxValueAsync(user.Id, Achievements.MealIngredientAchievement.AdditionsCountOver);

            if (user.CreatedMealIngredientsCount <= currentlyBestValue)
            {
                return;
            }

            int[] achievementStages = achievementsConfig.MealIngredientAchievements[Achievements.MealIngredientAchievement.AdditionsCountOver];

            await AddAchievementIfNextStageReachedAsync(
                user.Id,
                achievementStages,
                user.CreatedMealIngredientsCount, 
                Achievements.MealIngredientAchievement.AdditionsCountOver
                );
        }

        #endregion

        #region User Achievements

        public async Task CheckForUserAnniversaryAsync(User userBeforeLastLoginUpdate)
        {
            int lastLoginAndCreationDifferenceInYears = Extensions.GetDifferenceInYears(
                                                            userBeforeLastLoginUpdate.LastLoginDate,
                                                            userBeforeLastLoginUpdate.CreationDate
                                                        );
            int currentDateAndCreationDifferenceInYears = Extensions.GetDifferenceInYears(
                                                              DateTimeOffset.Now,
                                                              userBeforeLastLoginUpdate.CreationDate
                                                          );

            if (lastLoginAndCreationDifferenceInYears == currentDateAndCreationDifferenceInYears)
            {
                return ;
            }

            int currentlyBestValue = await _userAchievementRepository.GetUserAchievementMaxValueAsync(userBeforeLastLoginUpdate.Id, Achievements.UserAchievement.Anniversary);

            if (currentDateAndCreationDifferenceInYears <= currentlyBestValue)
            {
                return ;
            }

            int[] achievementStages = achievementsConfig.UserAchievements[Achievements.UserAchievement.Anniversary];

            await AddAchievementIfNextStageReachedAsync(
                    userBeforeLastLoginUpdate.Id,
                    achievementStages, 
                    currentDateAndCreationDifferenceInYears, 
                    Achievements.UserAchievement.Anniversary
                );
        }

        #endregion

        #region Meal Schedule Achievements

        public async Task CheckForConsequentScheduleUpdatesAsync(Guid userId)
        {
            var oldMaxStreakTask = _userAchievementRepository.GetUserAchievementMaxValueAsync(
                                   userId,
                                   Achievements.MealScheduleAchievement.ConsequentScheduleUpdates
                               );

            var newStreakTask = _mealScheduleRepository.GetMealScheduleUpdatesStreakInDaysAsync(userId);

            await Task.WhenAll(oldMaxStreakTask, newStreakTask);

            if (newStreakTask.Result <= oldMaxStreakTask.Result)
            {
                return;
            }

            int[] achievementStages = achievementsConfig.MealScheduleAchievements[Achievements.MealScheduleAchievement.ConsequentScheduleUpdates];

            await AddAchievementIfNextStageReachedAsync(
                userId,
                achievementStages, 
                newStreakTask.Result, 
                Achievements.MealScheduleAchievement.ConsequentScheduleUpdates
                );
        }

        #endregion

        #region Friend Achievements

        public async Task CheckForNumberOfFriendsAsync(Guid userId)
        {
            var newValueTask = _friendRepository.GetNumberOfFriendsAsync(userId);

            var currentlyBestValueTask = _userAchievementRepository.GetUserAchievementMaxValueAsync(userId, Achievements.MealAchievement.NumberOfFavouriteMarks);

            await Task.WhenAll(newValueTask, currentlyBestValueTask);

            if (newValueTask.Result <= currentlyBestValueTask.Result)
            {
                return;
            }

            int[] achievementStages = achievementsConfig.FriendAchievements[Achievements.FriendAchievement.NumberOfFriends];

            await AddAchievementIfNextStageReachedAsync(
                userId,
                achievementStages,
                newValueTask.Result,
                Achievements.FriendAchievement.NumberOfFriends
                );
        }

        #endregion

        #region Private Methods

        private async Task AddAchievementIfNextStageReachedAsync(Guid userId, int[] achievementStages, int newValue, object achievement)
        {
            bool achievementReached = CheckIfAchievementReached(achievementStages, newValue);

            if (!achievementReached)
            {
                return ;
            }

            var dbAchievement = _achievementRepository.GetAchievement(achievement, newValue);

            var userAchievementCreation = new UserAchievementCreation(dbAchievement.Id, userId);

            await AddUserAchievement(userAchievementCreation, dbAchievement);
        }

        private static bool CheckIfAchievementReached(int[] stages, int newValue) => stages.Contains(newValue);

        private async Task AddUserAchievement<T>(T userAchievementCreation, Achievement achievement)
        {
            var dbUserAchievement = _mapper.Map<UserAchievement>(userAchievementCreation);

            bool insertCompleted = await _userAchievementRepository.AddAsync(dbUserAchievement);

            ThrowIfNotCompleted(insertCompleted, dbUserAchievement);

            dbUserAchievement.Achievement = achievement;

            await _activityService.LogNewAchievementReachedAsync(dbUserAchievement.UserId, achievement.Id);
        }

        private static void ThrowIfNotCompleted(bool insertCompleted, object model)
        {
            if (!insertCompleted)
            {
                throw new DataAccessException($"Adding achievement ${JsonConvert.SerializeObject(model)} failed");
            }
        }

        #endregion
    }
}
