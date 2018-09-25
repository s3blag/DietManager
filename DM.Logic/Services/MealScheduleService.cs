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
            return _mapper.Map<IEnumerable<MealScheduleEntryVM>>(
                await _mealScheduleRepository.GetMealScheduleEntriesInDateRangeAsync(
                    userId, 
                    dateOffset, 
                    DateTimeOffset.Now.AddDays(MEAL_SCHEDULE_FETCH_RANGE_IN_DAYS)
                    ));
        }

        public async Task<Guid> AddMealScheduleEntry(Guid userId, NewMealScheduleEntryVM newMealScheduleEntry)
        {
            var dbMealScheduleEntry = _mapper.Map<MealScheduleEntry>(newMealScheduleEntry);
            dbMealScheduleEntry.UserId = userId;

            var result = await _mealScheduleRepository.AddMealScheduleEntryAsync(dbMealScheduleEntry);

            if (result == true)
            {
                return dbMealScheduleEntry.Id;
            }

            return Guid.Empty;
        }
    }
}
