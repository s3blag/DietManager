using DM.Database;
using DM.Repositories.Interfaces;
using LinqToDB;
using System;
using System.Threading.Tasks;

namespace DM.Repositories
{
    public abstract class BaseRepository<T> : IBaseRepository<T> where T: class
    {
        public virtual async Task<bool> AddAsync(T model)
        {
            using (var db = new DietManagerDB())
            {
                int result = await db.InsertAsync(model);

                return Convert.ToBoolean(result);
            }
        }

        public virtual async Task<bool> DeleteAsync(T model)
        {
            using (var db = new DietManagerDB())
            {
                int result = await db.DeleteAsync(model);

                return Convert.ToBoolean(result);
            }
        }

    }
}
