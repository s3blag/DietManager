using DM.Database;
using DM.Repositories.Interfaces;
using LinqToDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DM.Repositories
{
    public class FavouriteRepository: BaseRepository<Favourite>, IFavouriteRepository
    {
        public async Task<IDictionary<Guid, int>> GetNumberOfFavouritesMarksAsync(IEnumerable<Guid> mealIds)
        {
            using (var db = new DietManagerDB())
            {
                return await db.Favourites.
                    GroupBy(f => f.MealId).
                    Having(_ => mealIds.Contains(_.Key)).
                    ToDictionaryAsync(_ => _.Key, _ => _.Count());
            }
        }

        public async Task<IList<Favourite>> GetUserFavouritesAsync(Guid userId, int index, int takeAmount)
        {
            using (var db = new DietManagerDB())
            {
                return await db.Favourites.
                    LoadWith(f => f.Meal).
                    LoadWith(f => f.User).
                    Where(f => f.UserId == userId).
                    Skip(index).
                    Take(takeAmount).
                    ToListAsync();
            }
        }
    }
}
