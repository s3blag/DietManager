using DM.Database;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DM.Repositories
{
    public class MealRepository
    {
        public async Task<Meal> GetMealAsync(Guid id)
        {
            if (id == Guid.Empty)
            {
                throw new ArgumentNullException($"No {nameof(Meal)} for given guid: {id}");
            }

            return null;
        }
    }
}
