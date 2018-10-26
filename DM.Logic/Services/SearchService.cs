using AutoMapper;
using DM.Logic.Interfaces;
using DM.Models.ViewModels;
using DM.Models.Wrappers;
using DM.Repositories.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DM.Logic.Services
{
    public class SearchService : ISearchService
    {
        private readonly IMealRepository _mealRepository;
        private readonly IMealIngredientRepository _mealIngredientRepository;
        private readonly IMealIngredientsApiCaller _mealIngredientsApi;
        private readonly IMapper _mapper;

        public SearchService(IMealRepository mealRepository, IMealIngredientRepository mealIngredientRepository, 
            IMealIngredientsApiCaller mealIngredientsApi, IMapper mapper)
        {
            _mealRepository = mealRepository;
            _mealIngredientRepository = mealIngredientRepository;
            _mealIngredientsApi = mealIngredientsApi;
            _mapper = mapper;
        }

        public async Task<IndexedResult<IEnumerable<MealPreviewVM>>> SearchMealAsync(
            IndexedResult<MealSearchVM> searchArgumentsVM,
            int takeAmount = DbConstants.DEFAULT_DB_TAKE_VALUE)
        {
            var searchResult = await _mealRepository.GetMealPreviewsByQueryAsync(
                searchArgumentsVM.Result.Query, 
                searchArgumentsVM.Index, 
                takeAmount
            );

            return new IndexedResult<IEnumerable<MealPreviewVM>>
            {
                Result = _mapper.Map<IEnumerable<MealPreviewVM>>(searchResult),
                Index = searchArgumentsVM.Index + searchResult.Count,
                IsLast = searchResult.Count != takeAmount
            };

        }

        public async Task<IndexedResult<IEnumerable<MealIngredientVM>>> SearchMealIngredientAsync(
            IndexedResult<MealIngredientSearchVM> searchArgumentsVM,
            int takeAmount = DbConstants.DEFAULT_DB_TAKE_VALUE)
        {
            var searchResult = _mapper.Map<IList<MealIngredientVM>>(
                await _mealIngredientRepository.GetMealIngredientsByQueryAsync(
                    searchArgumentsVM.Result.Query,
                    searchArgumentsVM.Index,
                    takeAmount)
            );

            if (!searchResult.Any())
            {
                searchResult = await _mealIngredientsApi.GetMealIngredientsByQueryAsync(searchArgumentsVM.Result.Query);
                //save to the database
            }

            return new IndexedResult<IEnumerable<MealIngredientVM>>
            {
                Result = searchResult,
                Index = searchArgumentsVM.Index + searchResult.Count,
                IsLast = searchResult.Count != takeAmount
            };
        }
    }
}
