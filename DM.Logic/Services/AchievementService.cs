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
        private readonly IFavouriteRepository _favouriteRepository;
        private readonly IMealIngredientRepository _mealIngredientRepository;
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
            _favouriteRepository = favouriteRepository;
            _mealIngredientRepository = mealIngredientRepository;
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
            var newValueTask = _mealScheduleRepository.GetNumberOfMealUsesAsync(mealId);

            var currentlyBestValueTask = _userAchievementRepository.GetUserAchievementMaxValueAsync(userId, Achievements.MealAchievement.NumberOfUses);

            await Task.WhenAll(newValueTask, currentlyBestValueTask);

            if (newValueTask.Result <= currentlyBestValueTask.Result)
            {
                return;
            }

            int[] achievementStages = achievementsConfig.MealAchievements[Achievements.MealAchievement.NumberOfUses];

            await AddAchievementIfNextStageReachedAsync(userId, achievementStages, newValueTask.Result, Achievements.MealAchievement.NumberOfUses);
        }


        public async Task<UserAchievementVM> CheckForNumberOfMealAdditionsByUserAsync(Guid userId)
        {
            int newValue = await _mealRepository.GetMealsCreatedByUserCountAsync(userId);

            int[] achievementStages = achievementsConfig.MealAchievements[Achievements.MealAchievement.AdditionsCountOver];

            return _mapper.Map<UserAchievementVM>(await AddAchievementIfNextStageReachedAsync(userId, achievementStages, newValue, Achievements.MealAchievement.AdditionsCountOver));
        }

        public async Task CheckForNumberOfFavouriteMarksAsync(Guid mealId)
        {
            int[] achievementStages = achievementsConfig.MealAchievements[Achievements.MealAchievement.NumberOfFavouriteMarks];

            var newValueTask = _favouriteRepository.GetNumberOfFavouritesMarksAsync(new[] { mealId });

            var creatorIdTask =  _mealRepository.GetMealByIdAsync(mealId);

            await Task.WhenAll(newValueTask, creatorIdTask);

            var creatorId = creatorIdTask.Result.CreatorId;
            int newValue = newValueTask.Result.First().Value;

            int currentlyBestValue = await _userAchievementRepository.GetUserAchievementMaxValueAsync(creatorId, Achievements.MealAchievement.NumberOfFavouriteMarks);

            if (newValue <= currentlyBestValue)
            {
                return;
            }

            await AddAchievementIfNextStageReachedAsync(creatorId, achievementStages, newValue, Achievements.MealAchievement.NumberOfFavouriteMarks);
        }

        #endregion

        #region MealIngredient Achievements

        public async Task<UserAchievementVM> CheckForNumberOfMealIngredientAdditionsByUserAsync(Guid userId)
        {
            int newValue = await _mealIngredientRepository.GetMealIngredientsAddedByUserCountAsync(userId);

            int[] achievementStages = achievementsConfig.MealIngredientAchievements[Achievements.MealIngredientAchievement.AdditionsCountOver];

            return _mapper.Map<UserAchievementVM>(await AddAchievementIfNextStageReachedAsync(
                userId,
                achievementStages, 
                newValue, 
                Achievements.MealIngredientAchievement.AdditionsCountOver
                ));
        }

        #endregion

        #region User Achievements

        public async Task<UserAchievementVM> CheckForUserAnniversaryAsync(User userBeforeLastLoginUpdate)
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
                return null;
            }

            int[] achievementStages = achievementsConfig.UserAchievements[Achievements.UserAchievement.Anniversary];

            return _mapper.Map<UserAchievementVM>(await AddAchievementIfNextStageReachedAsync(
                    userBeforeLastLoginUpdate.Id,
                    achievementStages, 
                    currentDateAndCreationDifferenceInYears, 
                    Achievements.UserAchievement.Anniversary
                ));
        }

        #endregion

        #region Meal Schedule Achievements

        public async Task<UserAchievementVM> CheckForConsequentScheduleUpdatesAsync(Guid userId)
        {
            int oldMaxStreak = await _userAchievementRepository.GetUserAchievementMaxValueAsync(
                                   userId,
                                   Achievements.MealScheduleAchievement.ConsequentScheduleUpdates
                               );

            int newStreak = await _mealScheduleRepository.GetMealScheduleUpdatesStreakInDaysAsync(userId);

            if (newStreak <= oldMaxStreak)
            {
                return null;
            }

            int[] achievementStages = achievementsConfig.MealScheduleAchievements[Achievements.MealScheduleAchievement.ConsequentScheduleUpdates];

            return _mapper.Map<UserAchievementVM>(await AddAchievementIfNextStageReachedAsync(
                userId,
                achievementStages, 
                newStreak, 
                Achievements.MealScheduleAchievement.ConsequentScheduleUpdates
                ));
        }

        #endregion

        #region Friend Achievements

        public async Task<UserAchievementVM> CheckForNumberOfFriendsAsync(Guid userId)
        {
            var newValueTask = _friendRepository.GetNumberOfFriendsAsync(userId);

            var currentlyBestValueTask = _userAchievementRepository.GetUserAchievementMaxValueAsync(userId, Achievements.MealAchievement.NumberOfFavouriteMarks);

            await Task.WhenAll(newValueTask, currentlyBestValueTask);

            if (newValueTask.Result <= currentlyBestValueTask.Result)
            {
                return null;
            }

            int[] achievementStages = achievementsConfig.FriendAchievements[Achievements.FriendAchievement.NumberOfFriends];

            return _mapper.Map<UserAchievementVM>(await AddAchievementIfNextStageReachedAsync(
                userId,
                achievementStages,
                newValueTask.Result,
                Achievements.FriendAchievement.NumberOfFriends
                ));
        }

        #endregion

        #region Private Methods

        private async Task<UserAchievement> AddAchievementIfNextStageReachedAsync(Guid userId, int[] achievementStages, int newValue, object achievement)
        {
            bool achievementReached = CheckIfAchievementReached(achievementStages, newValue);

            if (!achievementReached)
            {
                return null;
            }

            var dbAchievement = _achievementRepository.GetAchievement(achievement, newValue);

            var userAchievementCreation = new UserAchievementCreation(dbAchievement.Id, userId);

            return await AddUserAchievement(userAchievementCreation, dbAchievement);
        }

        private static bool CheckIfAchievementReached(int[] stages, int newValue) => stages.Contains(newValue);

        private async Task<UserAchievement> AddUserAchievement<T>(T userAchievementCreation, Achievement achievement)
        {
            var dbUserAchievement = _mapper.Map<UserAchievement>(userAchievementCreation);

            bool insertCompleted = await _userAchievementRepository.AddAsync(dbUserAchievement);

            ThrowIfNotCompleted(insertCompleted, dbUserAchievement);

            dbUserAchievement.Achievement = achievement;

            await _activityService.LogNewAchievementReachedAsync(dbUserAchievement.UserId, dbUserAchievement.Id);

            return dbUserAchievement;
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
