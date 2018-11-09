using AutoMapper;
using DM.Database;
using DM.Logic.Interfaces;
using DM.Models.Exceptions;
using DM.Models.ViewModels;
using DM.Repositories.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public async Task<Dictionary<DayOfWeek, IEnumerable<MealScheduleEntryVM>>> GetUpcomingMealSchedule(Guid userId, DateTimeOffset dateOffset)
        {
            ValidateArgument(
                (userId, nameof(userId)), 
                (dateOffset, nameof(dateOffset))
                );

            var mealScheduleEntries = _mapper.Map<IEnumerable<MealScheduleEntryVM>>(
                        await _mealScheduleRepository.GetMealScheduleEntriesInDateRangeAsync(
                                                        userId,
                                                        dateOffset,
                                                        DateTimeOffset.Now.AddDays(Constants.MEAL_SCHEDULE_FETCH_RANGE_IN_DAYS))
            );

            var groupedMealScheduleEntries = mealScheduleEntries.
                                                GroupBy(m => m.Date.DayOfWeek).
                                                ToDictionary(kv => kv.Key, kv => kv.AsEnumerable());

            return groupedMealScheduleEntries;
        }

        public async Task<Guid> AddMealScheduleEntry(Guid userId, MealScheduleEntryCreationVM newMealScheduleEntry)
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

            var checkNumberOfMealUsesTask = _achievementService.CheckForNumberOfMealUsesAsync(userId, dbMealScheduleEntry.MealId);
            var checkConsequentUpdatesTask =  _achievementService.CheckForConsequentScheduleUpdatesAsync(userId);

            await Task.WhenAll(checkNumberOfMealUsesTask, checkConsequentUpdatesTask);

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
