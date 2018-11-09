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
        private readonly IAchievementService _achievementService;
        private readonly IActivityService _activityService;

        public MealIngredientService(IMapper mapper, IMealIngredientRepository mealIngredientRepository,
                                     IAchievementService achievementService, IActivityService activityService)
        {
            _mapper = mapper;
            _mealIngredientRepository = mealIngredientRepository;
            _achievementService = achievementService;
            _activityService = activityService;
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

        public async Task<Guid> AddMealIngredientAsync(Guid userId, MealIngredientCreationVM mealIngredient)
        {
            ValidateArgument(mealIngredient, nameof(mealIngredient));

            var dbMealIngredient = _mapper.Map<MealIngredient>(mealIngredient);
            dbMealIngredient.CreatorId = userId;

            bool mealIngredientNutritionsAddedSuccessfully = await _mealIngredientRepository.AddMealIngredientNutritionsAsync(dbMealIngredient.Nutrition);

            if (!mealIngredientNutritionsAddedSuccessfully)
            {
                throw new DataAccessException("Failed adding nutritions: " + JsonConvert.SerializeObject(dbMealIngredient.Nutrition) + "for mealIngredient: "
                    + JsonConvert.SerializeObject(dbMealIngredient));
            }

            dbMealIngredient.NutritionsId = dbMealIngredient.Nutrition.Id;

            bool mealIngredientAddedSuccesfully =  await _mealIngredientRepository.AddAsync(dbMealIngredient);

            if (!mealIngredientAddedSuccesfully)
            {
                throw new DataAccessException("MealIngredient couldn't be added. MealIngredient:" + JsonConvert.SerializeObject(dbMealIngredient));
            }

            var logNewMealIngredientAddedTask = _activityService.LogNewMealIngredientAddedAsync(userId, dbMealIngredient.Id);
            var checkForNumberOfMealIngredientAdditionsTask = _achievementService.CheckForNumberOfMealIngredientAdditionsByUserAsync(userId);

            await Task.WhenAll(logNewMealIngredientAddedTask, checkForNumberOfMealIngredientAdditionsTask);
            
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
