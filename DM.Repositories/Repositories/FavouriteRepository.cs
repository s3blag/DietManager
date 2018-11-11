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

        public async Task<IList<Favourite>> GetUserFavouritesAsync(Guid userId, int index, int takeAmount, string nameFilter = null)
        {
            using (var db = new DietManagerDB())
            {
                var favouritesQuery = db.Favourites.
                    LoadWith(f => f.Meal).
                    LoadWith(f => f.User).
                    Where(f => f.UserId == userId).
                    AsQueryable();

                if (nameFilter != null)
                {
                    favouritesQuery = favouritesQuery.Where(f => f.Meal.Name.ToLower().Contains(nameFilter));
                }

                return await favouritesQuery.
                    Skip(index).
                    Take(takeAmount).
                    ToListAsync();

            }
        }

        public async Task<bool> DeleteAsync(Guid userId, Guid mealId)
        {
            using (var db = new DietManagerDB())
            {
                var result = await db.Favourites.
                    Where(f => f.MealId == mealId).
                    Where(f => f.UserId == userId).
                    DeleteAsync();
                return result == 1;
            }
        }
    }
}
