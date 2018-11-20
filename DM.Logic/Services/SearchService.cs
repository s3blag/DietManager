using AutoMapper;
using DM.Logic.Interfaces;
using DM.Models.ViewModels;
using DM.Models.Wrappers;
using DM.Repositories.Interfaces;
using System;
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
        private readonly IUserRepository _userRepository;
        private readonly IFavouriteRepository _favouritesRepository;
        private readonly IMapper _mapper;

        public SearchService(IMealRepository mealRepository, IMealIngredientRepository mealIngredientRepository, 
            IMealIngredientsApiCaller mealIngredientsApi, IUserRepository userRepository, IMapper mapper,
            IFavouriteRepository favouriteRepository)
        {
            _mealRepository = mealRepository;
            _favouritesRepository = favouriteRepository;
            _mealIngredientRepository = mealIngredientRepository;
            _mealIngredientsApi = mealIngredientsApi;
            _userRepository= userRepository;
            _mapper = mapper;
        }

        public async Task<IndexedResult<IEnumerable<MealPreviewVM>>> SearchMealsAsync(
            Guid userId,
            IndexedResult<MealSearchVM> searchArgumentsVM,
            int takeAmount = Constants.DEFAULT_DB_TAKE_VALUE)
        {
            var searchTask = _mealRepository.GetMealPreviewsByQueryAsync(
                searchArgumentsVM.Result.Query,
                searchArgumentsVM.Index,
                takeAmount
            );

           
            var favouritesTask = _favouritesRepository.GetUserFavouritesAsync(
                                    userId,
                                    0,
                                    int.MaxValue,
                                    searchArgumentsVM.Result.Query);

            await Task.WhenAll(searchTask, favouritesTask);

            if (!searchTask.Result.Any())
            {
                return new IndexedResult<IEnumerable<MealPreviewVM>>
                {
                    Result = Enumerable.Empty<MealPreviewVM>(),
                    Index = 0,
                    IsLast = true
                };
            }

            var mealFavouriteCounts = await _favouritesRepository.GetNumberOfFavouritesMarksAsync(searchTask.Result.Select(m => m.Id));

            SetNumberOfFavouriteMarks(searchTask.Result, mealFavouriteCounts);

            var searchResultVM = _mapper.Map<IEnumerable<MealPreviewVM>>(searchTask.Result);

            SetIsFavouriteField(userId, favouritesTask.Result, searchResultVM);

            return new IndexedResult<IEnumerable<MealPreviewVM>>
            {
                Result = searchResultVM,
                Index = searchArgumentsVM.Index + searchTask.Result.Count,
                IsLast = searchTask.Result.Count != takeAmount
            };

        }      

        public async Task<IndexedResult<IEnumerable<MealIngredientVM>>> SearchMealIngredientsAsync(
            IndexedResult<MealIngredientSearchVM> searchArgumentsVM,
            int takeAmount = Constants.DEFAULT_DB_TAKE_VALUE)
        {
            var searchResult = _mapper.Map<ICollection<MealIngredientVM>>(
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

        public async Task<IndexedResult<IEnumerable<UserVM>>> SearchUsersAsync(
            IndexedResult<UserSearchVM> searchArgumentsVM,
            int takeAmount = Constants.DEFAULT_DB_TAKE_VALUE)
        {
            var searchResult = _mapper.Map<ICollection<UserVM>>(
                await _userRepository.GetUsersByQueryAsync(
                    searchArgumentsVM.Result.Query,
                    searchArgumentsVM.Index,
                    takeAmount)
            );

            return new IndexedResult<IEnumerable<UserVM>>
            {
                Result = searchResult,
                Index = searchArgumentsVM.Index + searchResult.Count,
                IsLast = searchResult.Count != takeAmount
            };
        }

        private void SetNumberOfFavouriteMarks(ICollection<Models.Models.MealPreview> userMealPreviews, IDictionary<Guid, int> mealFavouriteCounts)
        {
            if (mealFavouriteCounts.Any())
            {
                foreach (var mealPreview in userMealPreviews)
                {
                    if (mealFavouriteCounts.TryGetValue(mealPreview.Id, out int favouritesCount))
                    {
                        mealPreview.NumberOfFavouriteMarks = favouritesCount;
                    }
                }
            }
        }

        private void SetIsFavouriteField(
            Guid userId, 
            ICollection<Database.Favourite> favourites, 
            IEnumerable<MealPreviewVM> searchResultVM)
        {
            var commonMealIds = searchResultVM.Select(m => m.Id.Value).
                            Intersect(favourites.Select(f => f.MealId)).
                            ToList();

            if (commonMealIds.Any())
            {
                foreach (var mealPreview in searchResultVM.Where(m => commonMealIds.Contains(m.Id.Value)))
                {
                    mealPreview.IsFavourite = true;
                }
            }

            foreach (var mealPreview in searchResultVM)
            {
                if (mealPreview.Creator.Id != userId && mealPreview.IsFavourite != true)
                {
                    mealPreview.IsFavourite = false;
                }
            }
        }
    }
}
