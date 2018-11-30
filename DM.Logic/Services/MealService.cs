using AutoMapper;
using DM.Database;
using DM.Logic.Interfaces;
using DM.Models;
using DM.Models.Exceptions;
using DM.Models.ViewModels;
using DM.Models.Wrappers;
using DM.Repositories.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DM.Logic.Services
{
    public class MealService : IMealService
    {
        private readonly IMapper _mapper;
        private readonly IMealRepository _mealRepository;
        private readonly IMealIngredientRepository _mealIngredientRepository;
        private readonly IFavouriteRepository _favouritesRepository;
        private readonly IAchievementService _achievementService;
        private readonly IActivityService _activityService;
        private readonly IUserRepository _userRepository;

        public MealService(IMapper mapper, IMealRepository mealRepository, IMealIngredientRepository mealIngredientRepository, 
            IFavouriteRepository favouritesRepository, IAchievementService achievementService, IActivityService activityService,
            IUserRepository userRepository)
        {
            _mapper = mapper;
            _mealRepository = mealRepository;
            _mealIngredientRepository = mealIngredientRepository;
            _favouritesRepository = favouritesRepository;
            _achievementService = achievementService;
            _activityService = activityService;
            _userRepository = userRepository;
        }

        public async Task<MealVM> GetMealByIdAsync(Guid userId, Guid mealId)
        {
            var meal = await _mealRepository.GetMealByIdAsync(mealId);

            if (meal == null)
            {
                return null;
            }

            var ingredients = await _mealIngredientRepository.GetMealIngredientsForMealAsync(mealId);

            var mealVM = _mapper.Map<MealVM>(new MealWithIngredients(meal, ingredients));

            if (meal.CreatorId != userId)
            {
                if (await _favouritesRepository.ContainsAsync(userId, mealId))
                {
                    mealVM.IsFavourite = true;
                } else
                {
                    mealVM.IsFavourite = false;
                }

            }

            return mealVM;
        }

        public async Task<Guid> AddMealAsync(MealCreationVM mealVM, Guid userId)
        {
            //TODO: ValidateArguments((mealVM, nameof(mealVM)));

            var dbMeal = _mapper.Map<Meal>(mealVM);
            dbMeal.CreatorId = userId;

            if (!await _mealRepository.AddAsync(dbMeal))
            {
                throw new DataAccessException(
                    nameof(_mealRepository.AddAsync) + 
                    " failed for argument: " + 
                    JsonConvert.SerializeObject(dbMeal)
                );
            }

            if (!await _mealRepository.AddMealMealIngredientsAsync(GetMealMealIngredients(dbMeal.Id, mealVM.IngredientsIdsWithQuantity)))
            {      
                throw new DataAccessException(
                    nameof(_mealRepository.AddMealMealIngredientsAsync) + 
                    " failed for argument: " + 
                    JsonConvert.SerializeObject(GetMealMealIngredients(dbMeal.Id, mealVM.IngredientsIdsWithQuantity))
                );
            }

            await _userRepository.IncrementCreatedMealsCountAsync(userId);

            var userTask = _userRepository.GetUserByIdAsync(userId);
            var logNewMealAddedTask = _activityService.LogNewMealAddedAsync(userId, dbMeal.Id);

            await Task.WhenAll(logNewMealAddedTask, userTask);

            await _achievementService.CheckForNumberOfMealAdditionsByUserAsync(userTask.Result);

            return dbMeal.Id;
        } 

        public async Task<IndexedResult<IEnumerable<MealPreviewVM>>> GetUsersMealsPreviewsAsync(
            Guid userId, 
            IndexedResult<MealPreviewVM> lastReturned, 
            int takeAmount = Constants.DEFAULT_DB_TAKE_VALUE
        )
        {
            var userMealPreviews = await _mealRepository.GetMealPreviewsAsync(userId, lastReturned?.Index ?? 0, takeAmount);

            var userFavourites = await _favouritesRepository.GetUserFavouritesAsync(
                                   userId,
                                   0,
                                   int.MaxValue);

            var mealPreviewsVM = _mapper.Map<IEnumerable<MealPreviewVM>>(userMealPreviews);

            SetIsFavourite(userFavourites, mealPreviewsVM);

            return new IndexedResult<IEnumerable<MealPreviewVM>>()
            {
                Result = mealPreviewsVM,
                Index = (lastReturned?.Index ?? 0) + userMealPreviews.Count,
                IsLast = userMealPreviews.Count != takeAmount
            };
        }

        private void SetIsFavourite(ICollection<Favourite> favourites, IEnumerable<MealPreviewVM> mealPreviewsVM)
        {
            var commonMealIds = mealPreviewsVM.Select(m => m.Id.Value).
                            Intersect(favourites.Select(f => f.MealId)).
                            ToList();

            if (commonMealIds.Any())
            {
                foreach (var mealPreview in mealPreviewsVM.Where(m => commonMealIds.Contains(m.Id.Value)))
                {
                    mealPreview.IsFavourite = true;
                }
            }
        }

        private ICollection<MealMealIngredient> GetMealMealIngredients(
            Guid mealId,
            IEnumerable<MealIngredientIdWithQuantityVM> mealIngredientsIDs
        )
        {
            return mealIngredientsIDs.
                 Select(m =>
                     new MealMealIngredient()
                     {
                         Id = Guid.NewGuid(),
                         MealId = mealId,
                         MealIngredientId = m.Id.Value,
                         Quantity = m.Quantity.Value
                     }
                 ).
                 ToList();
        }

    }
}
