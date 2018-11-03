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
    public class MealScheduleService : IMealScheduleService
    {
        private readonly IMapper _mapper;
        private readonly IMealScheduleRepository _mealScheduleRepository;
        private readonly IAchievementService _achievementService;

        public MealScheduleService(IMapper mapper, IMealScheduleRepository mealScheduleRepository, IAchievementService achievementService)
        {
            _mapper = mapper;
            _mealScheduleRepository = mealScheduleRepository;
            _achievementService = achievementService;
        }

        public async Task<IEnumerable<MealScheduleEntryVM>> GetUpcomingMealSchedule(Guid userId, DateTimeOffset dateOffset)
        {
            ValidateArgument(
                (userId, nameof(userId)), 
                (dateOffset, nameof(dateOffset))
                );

            return _mapper.Map<IEnumerable<MealScheduleEntryVM>>(
                await _mealScheduleRepository.GetMealScheduleEntriesInDateRangeAsync(
                    userId, 
                    dateOffset, 
                    DateTimeOffset.Now.AddDays(Constants.MEAL_SCHEDULE_FETCH_RANGE_IN_DAYS)
                    ));
        }

        public async Task<Guid> AddMealScheduleEntry(Guid userId, NewMealScheduleEntryVM newMealScheduleEntry)
        {
            ValidateArgument(
                (userId, nameof(userId)),
                (newMealScheduleEntry, nameof(newMealScheduleEntry))
                );

            var dbMealScheduleEntry = _mapper.Map<MealScheduleEntry>(newMealScheduleEntry);
            dbMealScheduleEntry.UserId = userId;

            var addedSuccessfully = await _mealScheduleRepository.AddAsync(dbMealScheduleEntry);

            if (addedSuccessfully)
            {
                throw new DataAccessException($"Adding meal schedule entry failed for model: {JsonConvert.SerializeObject(dbMealScheduleEntry)}");
            }

            await _achievementService.CheckForNumberOfMealUsesAsync(userId, dbMealScheduleEntry.MealId);
            await _achievementService.CheckForConsequentScheduleUpdatesAsync(userId);

            return dbMealScheduleEntry.Id;
        }

        private void ValidateArgument(params (object value, string name)[] arguments)
        {
            foreach (var (value, name) in arguments)
            {
                if (value is null)
                {
                    throw new ArgumentNullException(name);
                }
            }
        }
    }
}
