using System;
using System.Threading.Tasks;
using DM.Models.ViewModels;

namespace DM.Logic.Interfaces
{
    public interface IMealService
    {
        Task<MealVM> AddMealAsync(NewMealVM mealVM);
        Task<MealVM> GetMealByIdAsync(Guid id);
    }
}