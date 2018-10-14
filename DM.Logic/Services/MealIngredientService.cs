using AutoMapper;
using DM.Database;
using DM.Logic.Interfaces;
using DM.Models.Exceptions;
using DM.Models.ViewModels;
using DM.Repositories.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DM.Logic.Services
{
    public class MealIngredientService : IMealIngredientService
    {
        private readonly IMapper _mapper;
        private readonly IMealIngredientRepository _mealIngredientRepository;

        public MealIngredientService(IMapper mapper, IMealIngredientRepository mealIngredientRepository)
        {
            _mapper = mapper;
            _mealIngredientRepository = mealIngredientRepository;
        }

        public async Task<IEnumerable<MealIngredientVM>> GetMealIngredientsForMealAsync(Guid mealId)
        {
            ValidateArgument(mealId, nameof(mealId));

            return _mapper.Map<IEnumerable<MealIngredientVM>>(await _mealIngredientRepository.GetMealIngredientsForMealAsync(mealId));
        }

        public async Task<MealIngredientVM> GetMealIngredientAsync(Guid mealIgredientId)
        {
            ValidateArgument(mealIgredientId, nameof(mealIgredientId));

            return _mapper.Map<MealIngredientVM>(await _mealIngredientRepository.GetMealIngredientByIdAsync(mealIgredientId));
        }

        public async Task<Guid> AddMealIngredientAsync(MealIngredientCreationVM mealIngredient)
        {
            ValidateArgument(mealIngredient, nameof(mealIngredient));

            var dbMealIngredient = _mapper.Map<MealIngredient>(mealIngredient);

            bool mealIngredientNutritionsAddedSuccessfully = await _mealIngredientRepository.AddMealIngredientNutritionsAsync(dbMealIngredient.Nutrition);

            if (!mealIngredientNutritionsAddedSuccessfully)
            {
                throw new DataAccessException("Failed adding nutritions: " + JsonConvert.SerializeObject(dbMealIngredient.Nutrition) + "for mealIngredient: " 
                    + JsonConvert.SerializeObject(dbMealIngredient));
            }

            bool mealIngredientAddedSuccesfully =  await _mealIngredientRepository.AddMealIngredientAsync(dbMealIngredient);

            if (!mealIngredientAddedSuccesfully)
            {
                throw new DataAccessException("MealIngredient couldn't be added. MealIngredient:" + JsonConvert.SerializeObject(dbMealIngredient));
            }

            return dbMealIngredient.Id;
        }

        private void ValidateArgument(Object argument, string argumentName)
        {
            if (argument == null)
            {
                throw new ArgumentNullException(argumentName);
            }
        }
    }
}
