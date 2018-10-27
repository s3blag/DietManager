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
    public class MealScheduleService : IMealScheduleService
    {
        private const int MEAL_SCHEDULE_FETCH_RANGE_IN_DAYS = 7;

        private readonly IMapper _mapper;
        private readonly IMealScheduleRepository _mealScheduleRepository;

        public MealScheduleService(IMapper mapper, IMealScheduleRepository mealScheduleRepository)
        {
            _mapper = mapper;
            _mealScheduleRepository = mealScheduleRepository;
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
                    DateTimeOffset.Now.AddDays(MEAL_SCHEDULE_FETCH_RANGE_IN_DAYS)
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
                return dbMealScheduleEntry.Id;
            }

            return Guid.Empty;
        }

        private void ValidateArgument(params (Object value, string name)[] arguments)
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
