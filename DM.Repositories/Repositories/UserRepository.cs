using DM.Database;
using LinqToDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DM.Repositories
{
    public class UserRepository
    {
        public async Task<User> GetUserAsync(Guid id)
        {
            using (var db = new DietManagerDB())
            {
                return await db.Users.
                    Where(u => u.Id == id).
                    FirstOrDefaultAsync()
                    ?? throw new KeyNotFoundException($"User with id = {id} was not found.");
            }
        }

        public async Task AddUserAsync(User user)
        {
            using (var db = new DietManagerDB())
            {
                await db.InsertAsync(user);
            }
        }

        public async Task DeleteUserAsync(Guid id)
        {
            using (var db = new DietManagerDB())
            {
                await db.Users.
                    Where(u => u.Id == id).
                    DeleteAsync();
            }
        }


    }
}
