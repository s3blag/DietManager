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

        #region MealAchievements

        public async Task CheckForNumberOfMealUsesAsync(Guid userId, Guid mealId)
        {
            int newValue = await _mealScheduleRepository.GetNumberOfMealUsesAsync(mealId);

            int[] achievementStages = achievementsConfig.MealAchievements[Achievements.MealAchievement.NumberOfUses];

            await AddAchievementIfNextStageReachedAsync(userId, achievementStages, newValue, Achievements.MealAchievement.NumberOfUses);
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

            int newValue = (await _favouriteRepository.GetNumberOfFavouritesMarksAsync(new[] { mealId })).First().Value;

            var creatorId = (await _mealRepository.GetMealByIdAsync(mealId)).CreatorId;
           
            await AddAchievementIfNextStageReachedAsync(creatorId, achievementStages, newValue, Achievements.MealAchievement.NumberOfFavouriteMarks);
        }

        #endregion

        #region MealIngredientAchievements

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

        #region UserAchievements

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

        #region MealScheduleAchievements

        public async Task<UserAchievementVM> CheckForConsequentScheduleUpdatesAsync(Guid userId)
        {
            int oldMaxStreak = await _userAchievementRepository.GetUserAchievementMaxValueAsync(
                                   userId,
                                   Achievements.MealScheduleAchievement.ConsequentScheduleUpdates
                               );

            int newStreak = await _mealScheduleRepository.GetMealScheduleUpdatesStreakInDaysAsync(userId);

            if (newStreak < oldMaxStreak)
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

        #region FriendAchievements

        public async Task<UserAchievementVM> CheckForNumberOfFriendsAsync(Guid userId)
        {
            int newValue = await _friendRepository.GetNumberOfFriendsAsync(userId);

            int[] achievementStages = achievementsConfig.FriendAchievements[Achievements.FriendAchievement.NumberOfFriends];

            return _mapper.Map<UserAchievementVM>(await AddAchievementIfNextStageReachedAsync(
                userId,
                achievementStages,
                newValue,
                Achievements.FriendAchievement.NumberOfFriends
                ));
        }

        #endregion

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

            return dbUserAchievement;
        }

        private static void ThrowIfNotCompleted(bool insertCompleted, object model)
        {
            if (!insertCompleted)
            {
                throw new DataAccessException($"Adding achievement ${JsonConvert.SerializeObject(model)} failed");
            }
        }

    }
}
