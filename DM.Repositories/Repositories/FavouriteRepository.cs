using DM.Database;
using DM.Repositories.Interfaces;
using LinqToDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DM.Repositories
{
    public class FavouriteRepository : BaseRepository<Favourite>, IFavouriteRepository
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

        public async Task<ICollection<Favourite>> GetUserFavouritesAsync(Guid userId, int index, int takeAmount, string nameFilter = null)
        {
            using (var db = new DietManagerDB())
            {
                var favouritesQuery = db.Favourites.
                    LoadWith(f => f.Meal).
                    LoadWith(f => f.Meal.Creator).
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
                db.BeginTransaction();
                bool succeeded;

                succeeded = (await db.Favourites.
                    Where(f => f.MealId == mealId).
                    Where(f => f.UserId == userId).
                    DeleteAsync() == 1);

                if (!succeeded)
                {
                    return false;
                }

                succeeded = (await db.Meals.
                    Where(m => m.Id == mealId).
                    Set(m => m.NumberOfFavouriteMarks, m => m.NumberOfFavouriteMarks -1).
                    UpdateAsync() == 1);
                
                db.CommitTransaction();

                return succeeded;
            }
        }

        public override async Task<bool> AddAsync(Favourite favourite)
        {
            using (var db = new DietManagerDB())
            {
                db.BeginTransaction();
                bool succeeded;

                succeeded = (await db.InsertAsync(favourite) == 1);

                if (!succeeded)
                {
                    return false;
                }

                succeeded = (await db.Meals.
                    Where(m => m.Id == favourite.MealId).
                    Set(m => m.NumberOfFavouriteMarks, m => m.NumberOfFavouriteMarks + 1).
                    UpdateAsync() == 1);

                db.CommitTransaction();

                return succeeded;
            }
        }
    }
}
