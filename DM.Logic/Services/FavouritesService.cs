using AutoMapper;
using DM.Database;
using DM.Logic.Interfaces;
using DM.Models.Exceptions;
using DM.Models.ViewModels;
using DM.Models.Wrappers;
using DM.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DM.Logic.Services
{
    public class FavouritesService : IFavouritesService
    {
        private readonly IFavouriteRepository _favouriteRepository;
        private readonly IAchievementService _achievementService;
        private readonly IMapper _mapper;
        private readonly IActivityService _activityService;
        private readonly IMealRepository _mealRepository;

        public FavouritesService(IFavouriteRepository favouriteRepository, IAchievementService achievementService,
            IMapper mapper, IActivityService activityService, IMealRepository mealRepository)
        {
            _favouriteRepository = favouriteRepository;
            _achievementService = achievementService;
            _mapper = mapper;
            _activityService = activityService;
            _mealRepository = mealRepository;
        }

        public async Task<IndexedResult<IEnumerable<MealPreviewVM>>> GetFavouriteMealsAsync(
            Guid userId, 
            IndexedResult<MealPreviewVM> lastReturned, 
            int takeAmount = Constants.DEFAULT_DB_TAKE_VALUE
            )
        {
            var usersFavourites = await _favouriteRepository.GetUserFavouritesAsync(userId, lastReturned?.Index ?? 0, takeAmount);

            return new IndexedResult<IEnumerable<MealPreviewVM>>()
            {
                Result = _mapper.Map<IEnumerable<MealPreviewVM>>(usersFavourites),
                Index = lastReturned?.Index ?? 0 + usersFavourites.Count,
                IsLast = usersFavourites.Count != takeAmount
            };
        }

        public async Task<Guid> AddToFavouritesAsync(FavouriteCreationVM favouriteCreation)
        {
            var dbFavourite = _mapper.Map<Favourite>(favouriteCreation);

            if (!await _favouriteRepository.AddAsync(dbFavourite))
            {
                throw new DataAccessException($"Adding to favourites failed for model: {dbFavourite}");
            }
           
            var checkForNumberOfFavouriteMarksTask = _achievementService.CheckForNumberOfFavouriteMarksAsync(dbFavourite.MealId);
            var LogNewFavouriteAddedTask = _activityService.LogNewFavouriteMealAddedAsync(dbFavourite.UserId, dbFavourite.MealId);

            await Task.WhenAll(checkForNumberOfFavouriteMarksTask, LogNewFavouriteAddedTask);

            return dbFavourite.Id;
        }

        public async Task<bool> RemoveFromFavouritesAsync(Guid userId, Guid mealId)
        {
            return await _favouriteRepository.DeleteAsync(userId, mealId);
        }
    }
}
