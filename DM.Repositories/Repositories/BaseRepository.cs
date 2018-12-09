using DM.Database;
using DM.Repositories.Interfaces;
using LinqToDB;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace DM.Repositories
{
    public abstract class BaseRepository<T> : IBaseRepository<T> where T: class
    {
        public virtual async Task<bool> AddAsync(T model)
        {
            using (var db = new DietManagerDB())
            {
                int rowsAffected = await db.InsertAsync(model);

                return rowsAffected == 1;
            }
        }

        public virtual async Task<bool> DeleteAsync(T model)
        {
            using (var db = new DietManagerDB())
            {
                int rowsAffected = await db.DeleteAsync(model);

                return rowsAffected == 1;
            }
        }

        public virtual async Task<bool> UpdateAsync(T model)
        {
            using (var db = new DietManagerDB())
            {
                int rowsAffected = await db.UpdateAsync(model);

                return rowsAffected == 1;
            }
        }

    }
}
