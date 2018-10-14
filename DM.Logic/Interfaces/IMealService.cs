using System;
using System.Threading.Tasks;
using DM.Models.ViewModels;

namespace DM.Logic.Interfaces
{
    public interface IMealService
    {
        Task<Guid> AddMealAsync(MealCreationVM mealVM);
        Task<MealVM> GetMealByIdAsync(Guid id);
    }
}