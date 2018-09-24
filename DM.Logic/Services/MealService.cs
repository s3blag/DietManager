using AutoMapper;
using DM.Database;
using DM.Logic.Interfaces;
using DM.Models;
using DM.Models.ViewModels;
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
            if (id == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(id));
            }

            var meal = await _mealRepository.GetMealByIdAsync(id);
            var ingredients = await _mealIngredientRepository.GetMealIngredientsForMealAsync(id);
            var mealVM = _mapper.Map<MealVM>(new MealWithIngredients(meal, ingredients));

            return mealVM;
        }

        public async Task<Guid> AddMealAsync(NewMealVM mealVM)
        {
            if (mealVM == null)
            {
                throw new ArgumentNullException(nameof(mealVM));
            }

            //addUser-Meal
            var dbMeal = _mapper.Map<Meal>(mealVM);

            var (mealIngredientsToInsert, mealIngredientNutritionsToInsert) = GetMealIngredientsToInsert(mealVM.Ingredients);

            if (await _mealIngredientRepository.AddMealIngredientNutritionsAsync(mealIngredientNutritionsToInsert) == false)
                throw new Exception(nameof(_mealIngredientRepository.AddMealIngredientNutritionsAsync) + " failed for argument: " + JsonConvert.SerializeObject(mealIngredientNutritionsToInsert));

            if (await _mealIngredientRepository.AddMealIngredientsAsync(mealIngredientsToInsert) == false)
                throw new Exception(nameof(_mealIngredientRepository.AddMealIngredientsAsync) + " failed for argument: " + JsonConvert.SerializeObject(mealIngredientsToInsert));

            if (await _mealRepository.AddMealAsync(dbMeal) == false)
                throw new Exception(nameof(_mealRepository.AddMealAsync) + " failed for argument: " + JsonConvert.SerializeObject(dbMeal));

            if (await _mealRepository.AddMealMealIngredientsAsync(GetMealMealIngredients(dbMeal.Id, mealVM.Ingredients)) == false)
                throw new Exception(nameof(_mealRepository.AddMealMealIngredientsAsync) + " failed for argument: " + JsonConvert.SerializeObject(GetMealMealIngredients(dbMeal.Id, mealVM.Ingredients)));

            return dbMeal.Id;
        } 

        private (IList<MealIngredient>, IList<Nutrition> nutritions) GetMealIngredientsToInsert(IEnumerable<MealIngredientVM> mealIngredientVMs)
        {
            var mealIngredientList = new List<MealIngredient>();
            var mealIngredientNutritionList = new List<Nutrition>();

            foreach (var mealIngredientVM in mealIngredientVMs)
            {
                if (mealIngredientVM.Id == null)
                {
                    var dbNutrition = _mapper.Map<Nutrition>(mealIngredientVM.Nutritions);
                    var dbMealIngredient = _mapper.Map<MealIngredient>(mealIngredientVM);
                    dbMealIngredient.NutritionsId = dbNutrition.Id;
                    mealIngredientVM.Id = dbMealIngredient.Id;

                    mealIngredientList.Add(dbMealIngredient);
                    mealIngredientNutritionList.Add(dbNutrition);
                }
            }

            return (mealIngredientList, mealIngredientNutritionList);
        }

        private IList<MealMealIngredient> GetMealMealIngredients(Guid mealId, IEnumerable<MealIngredientVM> mealIngredients) =>
            mealIngredients.Select(m => new MealMealIngredient() { Id = Guid.NewGuid(), MealId = mealId, MealIngredientId = m.Id.Value }).ToList();
    }
}
