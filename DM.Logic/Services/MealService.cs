using AutoMapper;
using DM.Database;
using DM.Logic.Interfaces;
using DM.Models.ViewModels;
using DM.Repositories.Interfaces;
using System;
using System.Threading.Tasks;

namespace DM.Logic.Services
{
    public class MealService : IMealService
    {
        private readonly IMapper _mapper;
        private readonly IMealRepository _mealRepository;

        public MealService(IMapper mapper, IMealRepository mealRepository)
        {
            _mapper = mapper;
            _mealRepository = mealRepository;
        }

        public async Task<MealVM> GetMealByIdAsync(Guid id)
        {
            if (id == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(id));
            }

            var mealWithIngredients = await _mealRepository.GetMealByIdAsync(id);

            var mealVM = _mapper.Map<MealVM>(mealWithIngredients);

            return mealVM;
        }

        public async Task<Guid> AddMealAsync(NewMealVM mealVM)
        {
            if (mealVM == null)
            {
                throw new ArgumentNullException(nameof(mealVM));
            }

            var dbMeal = _mapper.Map<Meal>(mealVM);

            //1. if mealIngredient is new add it, otherwise goto 2.
            //2. add mealMealIngredient
            var result = await _mealRepository.AddMealAsync(dbMeal);

            if (result != true)
            {
                return Guid.Empty;
            }

            return _mapper.Map<Guid>(dbMeal.Id);
        } 
    }
}
