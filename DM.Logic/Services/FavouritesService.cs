﻿using AutoMapper;
using DM.Database;
using DM.Logic.Interfaces;
using DM.Models.Exceptions;
using DM.Models.ViewModels;
using DM.Models.Wrappers;
using DM.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DM.Logic.Services
{
    public class FavouritesService : IFavouritesService
    {
        private readonly IFavouriteRepository _favouriteRepository;
        private readonly IAchievementService _achievementService;
        private readonly IMapper _mapper;

        public FavouritesService(IFavouriteRepository favouriteRepository, IAchievementService achievementService,
            IMapper mapper)
        {
            _favouriteRepository = favouriteRepository;
            _achievementService = achievementService;
            _mapper = mapper;
        }

        public async Task<IndexedResult<IEnumerable<FavouriteVM>>> GetFavouriteMealsAsync(
            Guid userId, 
            IndexedResult<FavouriteVM> lastReturned, 
            int takeAmount = Constants.DEFAULT_DB_TAKE_VALUE
            )
        {
            var usersFavourites = await _favouriteRepository.GetUserFavouritesAsync(userId, lastReturned?.Index ?? 0, takeAmount);

            return new IndexedResult<IEnumerable<FavouriteVM>>()
            {
                Result = _mapper.Map<IEnumerable<FavouriteVM>>(usersFavourites),
                Index = lastReturned?.Index ?? 0 + usersFavourites.Count,
                IsLast = usersFavourites.Count != takeAmount
            };
        }

        public async Task<Guid> AddToFavouritesAsync(FavouriteCreationVM favouriteCreation)
        {
            var dbFavourite = _mapper.Map<Favourite>(favouriteCreation);

            if (!await _favouriteRepository.AddAsync(dbFavourite))
            {
                throw new DataAccessException($"Adding to favourites failed for model: {dbFavourite}");
            }

            await _achievementService.CheckForNumberOfFavouriteMarksAsync(dbFavourite.MealId);

            return dbFavourite.Id;
        }

        public async Task RemoveFromFavouritesAsync(FavouriteVM favourite)
        {
            var dbFavourite = _mapper.Map<Favourite>(favourite);

            if (!await _favouriteRepository.DeleteAsync(dbFavourite))
            {
                throw new DataAccessException($"Adding to favourites failed for model: {dbFavourite}");
            }
        }
    }
}
