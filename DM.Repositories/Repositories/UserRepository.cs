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
        public async Task<User> GetUserByIdAsync(Guid id)
        {
            using (var db = new DietManagerDB())
            {
                return await db.Users.
                    Where(u => u.Id == id).
                    Where(u => !u.Deleted).
                    FirstOrDefaultAsync();
            }
        }

        public async Task<bool> IsUsernameUniqueAsync(string username)
        {
            using (var db = new DietManagerDB())
            {
                return !await db.Users.
                    Where(u => u.UserName == username).
                    AnyAsync();
            }
        }

        public async Task<User> GetUserByLoginDataAsync(string login)
        {
            using (var db = new DietManagerDB())
            {
                return await db.Users.
                    Where(u => u.UserName == login).
                    SingleOrDefaultAsync();
            }
        }

        public async Task<ICollection<User>> GetUsersByQueryAsync(string query, int index, int takeAmount)
        {
            using (var db = new DietManagerDB())
            {
                var userSearchQuery = db.Users.
                    Where(u => u.FullName.ToLower().Contains(query.ToLower())).
                    Where(u => !u.Deleted).
                    OrderBy(u => u.FullName).
                    ThenBy(u => u.CreationDate).
                    Skip(index).
                    Take(takeAmount);

                return await userSearchQuery.ToListAsync();
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

        public async Task<bool> UpdateUserAvatar(Guid userId, Guid? newAvatarId)
        {
            using (var db = new DietManagerDB())
            {
                int rowsAffected = await db.Users.
                   Where(u => u.Id == userId).
                   Set(u => u.ImageId, newAvatarId).
                   UpdateAsync();

                return rowsAffected == 1;
            }
        }

        public override async Task<bool> DeleteAsync(User model)
        {
            using (var db = new DietManagerDB())
            {
                int rowsAffected = await db.Users.
                   Where(u => u.Id == model.Id).
                   Set(u => u.ImageId, default(Guid?)).
                   Set(u => u.City, string.Empty).
                   Set(u => u.FullName, string.Empty).
                   Set(u => u.Name, string.Empty).
                   Set(u => u.Surname, string.Empty).
                   Set(u => u.UserName, Guid.NewGuid().ToString().Substring(0, 20)).
                   Set(u => u.Password, string.Empty).
                   Set(u => u.CreationDate, DateTimeOffset.MinValue).
                   Set(u => u.LastLoginDate, DateTimeOffset.MinValue).
                   Set(u => u.CreatedMealsCount, 0).
                   Set(u => u.CreatedMealIngredientsCount, 0).
                   Set(u => u.Deleted, true).
                   UpdateAsync();

                return rowsAffected == 1;
            }
        }

        public async Task DeleteUserRelatedDataAsync(User model)
        {
            using (var db = new DietManagerDB())
            {
                db.BeginTransaction();

                var achievementsTask = db.UserAchievements.
                    Where(ua => ua.UserId == model.Id).
                    DeleteAsync();

                var friendsTask = db.Friends.
                    Where(f => f.InvitedUserId == model.Id || f.InvitingUserId == model.Id).
                    DeleteAsync();

                var favouritesTask = db.Favourites.
                    Where(f => f.UserId == model.Id).
                    DeleteAsync();

                var activitiesTask = db.UserActivities.
                    Where(a => a.UserId == model.Id).
                    DeleteAsync();

                var scheduleTask = db.MealScheduleEntries.
                    Where(s => s.UserId == model.Id).
                    DeleteAsync();

                await Task.WhenAll(
                    achievementsTask,
                    friendsTask,
                    favouritesTask,
                    activitiesTask,
                    scheduleTask);

                db.CommitTransaction();
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
                   Set(u => u.CreatedMealIngredientsCount, u => u.CreatedMealIngredientsCount + valueToAdd).
                   UpdateAsync();
            }
        }

        #endregion
    }
}
