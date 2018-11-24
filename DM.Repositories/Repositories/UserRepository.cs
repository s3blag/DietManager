using DM.Database;
using DM.Repositories.Interfaces;
using LinqToDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DM.Repositories
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        public async Task<User> GetUserAsync(Guid id)
        {
            using (var db = new DietManagerDB())
            {
                return await db.Users.
                    Where(u => u.Id == id).
                    FirstOrDefaultAsync();
            }
        }

        public async Task<ICollection<User>> GetUsersByQueryAsync(string query, int index, int takeAmount)
        {
            using (var db = new DietManagerDB())
            {
                var mealPreviewsQuery = db.Users.
                    Where(u => u.FullName.ToLower().Contains(query)).
                    OrderBy(u => u.FullName).
                    ThenBy(u => u.CreationDate).
                    Skip(index).
                    Take(takeAmount);

                return await mealPreviewsQuery.ToListAsync();
            }
        }

        public async Task IncrementCreatedMealsCountAsync(Guid userId) => await UpdateCreatedMealsCountAsync(userId, 1);

        public async Task IncrementCreatedMealIngredientsCountAsync(Guid userId) => await UpdateCreatedMealIngredientsCountAsync(userId, 1);

        public async Task DecrementCreatedMealsCountAsync(Guid userId) => await UpdateCreatedMealsCountAsync(userId, -1);

        public async Task DecrementCreatedMealIngredientsCountAsync(Guid userId) => await UpdateCreatedMealIngredientsCountAsync(userId, -1);

        public async Task UpdateLastLoginDateAsync(Guid userId)
        {
            using (var db = new DietManagerDB())
            {
                await db.Users.
                   Where(u => u.Id == userId).
                   Set(u => u.LastLoginDate, DateTimeOffset.Now).
                   UpdateAsync();
            }
        }

        #region private

        private async Task UpdateCreatedMealsCountAsync(Guid userId, int valueToAdd)
        {
            using (var db = new DietManagerDB())
            {
                await db.Users.
                    Where(u => u.Id == userId).
                    Set(u => u.CreatedMealsCount, u => u.CreatedMealsCount + valueToAdd).
                    UpdateAsync();

            }
        }

        private async Task UpdateCreatedMealIngredientsCountAsync(Guid userId, int valueToAdd)
        {
            using (var db = new DietManagerDB())
            {
                await db.Users.
                   Where(u => u.Id == userId).
                   Set(u => u.CreatedMealIngredientsCount, u => u.CreatedMealsCount + valueToAdd).
                   UpdateAsync();
            }
        }

        #endregion
    }
}
