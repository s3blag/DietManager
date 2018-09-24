using AutoMapper;
using DM.Database;
using DM.Logic.Interfaces;
using DM.Models.ViewModels;
using DM.Repositories.Interfaces;
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
            return _mapper.Map<IEnumerable<MealIngredientVM>>(await _mealIngredientRepository.GetMealIngredientsForMealAsync(mealId));
        }

        public async Task<MealIngredientVM> GetMealIngredientAsync(Guid mealIgredientId)
        {
            return _mapper.Map<MealIngredientVM>(await _mealIngredientRepository.GetMealIngredientByIdAsync(mealIgredientId));
        }

        public async Task<Guid> AddMealIngredientAsync(MealIngredientVM mealIngredient)
        {
            if (mealIngredient == null)
            {
                throw new ArgumentNullException(nameof(mealIngredient));
            }

            var dbMealIngredient = _mapper.Map<MealIngredient>(mealIngredient);

            var addedSuccesfully =  await _mealIngredientRepository.AddMealIngredientAsync(dbMealIngredient);

            if (!addedSuccesfully)
            {
                return Guid.Empty;
            }

            return dbMealIngredient.Id;
        }
    }
}
