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

        public async Task<IEnumerable<Favourite>> GetUserFavouritesAsync(Guid userId)
        {
            using (var db = new DietManagerDB())
            {
                return await db.Favourites.
                    LoadWith(f => f.Meal).
                    Where(f => f.UserId == userId).
                    ToListAsync();
            }
        }

        public async Task<bool> RemoveFromFavouritesAsync(Favourite favouriteToRemove)
        {
            using (var db = new DietManagerDB())
            {
                return Convert.ToBoolean(await db.DeleteAsync(favouriteToRemove));
            }
        }
    }
}
