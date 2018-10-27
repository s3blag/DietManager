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

        public async Task<IList<User>> GetUsersByQueryAsync(string query, int index, int takeAmount)
        {
            using (var db = new DietManagerDB())
            {
                var mealPreviewsQuery = db.Users.
                    Where(m => m.FullName.ToLower().Contains(query)).
                    OrderBy(m => m.FullName).
                    ThenBy(m => m.CreationDate).
                    Skip(index).
                    Take(takeAmount);

                return await mealPreviewsQuery.ToListAsync();
            }
        }
    }
}
