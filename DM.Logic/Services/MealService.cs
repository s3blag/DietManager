using AutoMapper;
using DM.Database;
using DM.Logic.Interfaces;
using DM.Models;
using DM.Models.Exceptions;
using DM.Models.Models;
using DM.Models.ViewModels;
using DM.Models.ViewModels.Meal;
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

        public MealService(IMapper mapper, IMealRepository mealRepository, IMealIngredientRepository mealIngredientRepository, 
            IFavouriteRepository favouritesRepository)
        {
            _mapper = mapper;
            _mealRepository = mealRepository;
            _mealIngredientRepository = mealIngredientRepository;
            _favouritesRepository = favouritesRepository;
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

            if (!await _mealRepository.AddMealAsync(dbMeal))
            {
                throw new DataAccessException(
                    nameof(_mealRepository.AddMealAsync) + 
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

            return dbMeal.Id;
        } 

        public async Task DeleteMealAsync(Guid mealId)
        {
            ValidateArguments((mealId, nameof(mealId)));

            await _mealRepository.DeleteMealAsync(mealId);
        }

        public async Task<IndexedResult<IEnumerable<MealPreviewVM>>> GetMealPreviewsAsync(
            Guid userId, 
            IndexedResult<MealPreviewVM> lastReturned, 
            int takeAmount = DbConstants.DEFAULT_DB_TAKE_VALUE
        )
        {
            //TODO: uncomment after auth implementation

            //ValidateArguments(
            //    (userId, nameof(userId))
            //    );

            var mealPreviews = await _mealRepository.GetMealPreviewsAsync(userId, _mapper.Map<IndexedResult<MealPreview>>(lastReturned), takeAmount);

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
                Index = lastReturned?.Index ?? 0 + mealPreviews.Count,
                IsLast = mealPreviews.Count != takeAmount
            };           
        }

        private IList<MealMealIngredient> GetMealMealIngredients(
            Guid mealId, 
            IEnumerable<MealIngredientIdWithQuantityVM> mealIngredientsIDs
        ) =>    mealIngredientsIDs.Select(m => 
                    new MealMealIngredient()
                    {
                        Id = Guid.NewGuid(),
                        MealId = mealId,
                        MealIngredientId = m .Id,
                        Quantity = m.Quantity
                    }).
                ToList();

        private void ValidateArguments(params (Object value, string name)[] arguments)
        {
            foreach (var (value, name) in arguments)
            {
                if (value is Guid)
                {
                    if ((Guid)value == Guid.Empty)
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
