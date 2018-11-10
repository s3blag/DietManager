﻿using AutoMapper;
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

        public MealService(IMapper mapper, IMealRepository mealRepository, IMealIngredientRepository mealIngredientRepository, 
            IFavouriteRepository favouritesRepository, IAchievementService achievementService, IActivityService activityService)
        {
            _mapper = mapper;
            _mealRepository = mealRepository;
            _mealIngredientRepository = mealIngredientRepository;
            _favouritesRepository = favouritesRepository;
            _achievementService = achievementService;
            _activityService = activityService;
        }

        public async Task<MealVM> GetMealByIdAsync(Guid id)
        {
            ValidateArguments((id, nameof(id)));

            var meal = await _mealRepository.GetMealByIdAsync(id);
            var ingredients = await _mealIngredientRepository.GetMealIngredientsForMealAsync(id);
            var mealVM = _mapper.Map<MealVM>(new MealWithIngredients(meal, ingredients));

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

            var logNewMealAddedTask = _activityService.LogNewMealAddedAsync(userId, dbMeal.Id);
            var checkForNumberOfAdditionsTask = _achievementService.CheckForNumberOfMealAdditionsByUserAsync(userId);

            await Task.WhenAll(logNewMealAddedTask, checkForNumberOfAdditionsTask);

            return dbMeal.Id;
        } 

        public async Task<IndexedResult<IEnumerable<MealPreviewVM>>> GetUsersMealsPreviewsAsync(
            Guid userId, 
            IndexedResult<MealPreviewVM> lastReturned, 
            int takeAmount = Constants.DEFAULT_DB_TAKE_VALUE
        )
        {
            //TODO: uncomment after auth implementation

            //ValidateArguments(
            //    (userId, nameof(userId))
            //    );

            var mealPreviews = await _mealRepository.GetMealPreviewsAsync(userId, lastReturned?.Index ?? 0, takeAmount);

            var mealFavouritesCounts = await _favouritesRepository.GetNumberOfFavouritesMarksAsync(mealPreviews.Select(m => m.Id));

            if (mealFavouritesCounts.Any())
            {
                foreach (var mealPreview in mealPreviews)
                {
                    if (mealFavouritesCounts.TryGetValue(mealPreview.Id, out int favouritesCount))
                    {
                        mealPreview.NumberOfFavouriteMarks = favouritesCount;
                    } 
                }
            }

            return new IndexedResult<IEnumerable<MealPreviewVM>>()
            {
                Result = _mapper.Map<IEnumerable<MealPreviewVM>>(mealPreviews),
                Index = (lastReturned?.Index ?? 0) + mealPreviews.Count,
                IsLast = mealPreviews.Count != takeAmount
            };           
        }

        private IList<MealMealIngredient> GetMealMealIngredients(
            Guid mealId, 
            IEnumerable<MealIngredientIdWithQuantityVM> mealIngredientsIDs
        ) =>    mealIngredientsIDs.
                    Select(m => 
                        new MealMealIngredient()
                        {
                            Id = Guid.NewGuid(),
                            MealId = mealId,
                            MealIngredientId = m .Id.Value,
                            Quantity = m.Quantity.Value
                        }
                    ).
                    ToList();

        private void ValidateArguments(params (Object value, string name)[] arguments)
        {
            foreach (var (value, name) in arguments)
            {
                if (value is Guid id)
                {
                    if (id == Guid.Empty)
                    {
                        throw new ArgumentNullException(name);
                    }
                } 
                else
                {
                    if (value is null)
                    {
                        throw new ArgumentNullException(name);
                    }
                }
                    
            }
        }

    }
}
