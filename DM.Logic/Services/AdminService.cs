using AutoMapper;
using DM.Logic.Interfaces;
using DM.Models.ViewModels;
using DM.Models.Wrappers;
using DM.Repositories.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DM.Logic.Services
{
    public class AdminService : IAdminService
    {
        private readonly IMealRepository _mealRepository;
        private readonly IMealIngredientRepository _mealIngredientRepository;
        private readonly IActivityRepository _activityRepository;
        private readonly IImageService _imageService;
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        private readonly ILogger<FriendService> _logger;

        public AdminService(
            IMealRepository mealRepository,
            IMealIngredientRepository mealIngredientRepository,
            IActivityRepository activityRepository,
            IUserService userService,
            IImageService imageService,
            IMapper mapper,
            ILogger<FriendService> logger)
        {
            _mealRepository = mealRepository;
            _mealIngredientRepository = mealIngredientRepository;
            _activityRepository = activityRepository;
            _userService = userService;
            _imageService = imageService;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IndexedResult<IEnumerable<UserActivityVM>>> GetUsersActivitiesAsync(
           IndexedResult<UserActivityVM> lastReturned,
           int takeAmount = Constants.DEFAULT_DB_TAKE_VALUE)
        {

            var indexedUsersActivities = await _activityRepository.GetAllActivitiesAsync(
                                                                   lastReturned?.Index ?? 0,
                                                                   takeAmount);

            return new IndexedResult<IEnumerable<UserActivityVM>>()
            {
                Result = _mapper.Map<IEnumerable<UserActivityVM>>(indexedUsersActivities),
                Index = lastReturned?.Index ?? 0 + indexedUsersActivities.Count,
                IsLast = indexedUsersActivities.Count != takeAmount
            };
        }

        public async Task<bool> MarkActivitiesAsSeenAsync(IEnumerable<int> activitiesIds) => 
            await _activityRepository.MarkAsSeenAsync(activitiesIds);

        public async Task<bool> DeleteMealAsync(Guid mealId)
        {
            var meal = await _mealRepository.GetMealByIdAsync(mealId);

            if (meal == null)
            {
                return false;
            }

            if (meal.ImageId != null)
            {
                await _imageService.DeleteImageAsync(meal.ImageId.Value);
            }

            return await _mealRepository.MarkAsDeletedAsync(meal.Id);
        }

        public async Task<bool> DeleteMealIngredientAsync(Guid mealIngredientId) =>
             await _mealIngredientRepository.MarkAsDeletedAsync(mealIngredientId);

        public async Task<bool> DeleteUserAsync(Guid userId) =>
             await _userService.DeleteAccountAsync(userId);
    }
}
