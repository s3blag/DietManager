using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DM.Database;

namespace DM.Repositories.Interfaces
{
    public interface IUserRepository : IBaseRepository<User>
    {
        Task<User> GetUserByIdAsync(Guid id);
        Task<ICollection<User>> GetUsersByQueryAsync(string query, int index, int takeAmount);
        Task IncrementCreatedMealsCountAsync(Guid userId);
        Task IncrementCreatedMealIngredientsCountAsync(Guid userId);
        Task DecrementCreatedMealsCountAsync(Guid userId);
        Task DecrementCreatedMealIngredientsCountAsync(Guid userId);
        Task UpdateLastLoginDateAsync(Guid userId);
        Task<bool> UpdateUserAvatar(Guid userId, Guid? newAvatarId);
    }
}