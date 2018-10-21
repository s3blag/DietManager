using AutoMapper;
using DM.Database;
using DM.Logic.Interfaces;
using DM.Models;
using DM.Models.Exceptions;
using DM.Models.ViewModels;
using DM.Models.ViewModels.Meal;
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

        public MealService(IMapper mapper, IMealRepository mealRepository, IMealIngredientRepository mealIngredientRepository)
        {
            _mapper = mapper;
            _mealRepository = mealRepository;
            _mealIngredientRepository = mealIngredientRepository;
        }

        public async Task<MealVM> GetMealByIdAsync(Guid id)
        {
            ValidateArgument(id, nameof(id));

            var meal = await _mealRepository.GetMealByIdAsync(id);
            var ingredients = await _mealIngredientRepository.GetMealIngredientsForMealAsync(id);
            var mealVM = _mapper.Map<MealVM>(new MealWithIngredients(meal, ingredients));

            return mealVM;
        }

        public async Task<Guid> AddMealAsync(MealCreationVM mealVM)
        {
            ValidateArgument(mealVM, nameof(mealVM));

            //addUser-Meal(userId, newMealVM)
            var dbMeal = _mapper.Map<Meal>(mealVM);

            if (!await _mealRepository.AddMealAsync(dbMeal))
                throw new DataAccessException(nameof(_mealRepository.AddMealAsync) + " failed for argument: " + JsonConvert.SerializeObject(dbMeal));

            if (!await _mealRepository.AddMealMealIngredientsAsync(GetMealMealIngredients(dbMeal.Id, mealVM.IngredientsIdsWithQuantity)))
                throw new DataAccessException(nameof(_mealRepository.AddMealMealIngredientsAsync) + " failed for argument: " + JsonConvert.SerializeObject(GetMealMealIngredients(dbMeal.Id, mealVM.IngredientsIdsWithQuantity)));

            return dbMeal.Id;
        } 

        public async Task DeleteMealAsync(Guid mealId)
        {
            ValidateArgument(mealId, nameof(mealId));

            await _mealRepository.DeleteMealAsync(mealId);
        }

        private IList<MealMealIngredient> GetMealMealIngredients(Guid mealId, IEnumerable<MealIngredientIdWithQuantityVM> mealIngredientsIDs) =>
            mealIngredientsIDs.Select(m => new MealMealIngredient() { Id = Guid.NewGuid(), MealId = mealId, MealIngredientId = m .Id, Quantity = m.Quantity }).ToList();

        private void ValidateArgument(Object argument, string argumentName)
        {
            if (argument is null)
            {
                throw new ArgumentNullException(argumentName);
            }
        }
        
    }
}
