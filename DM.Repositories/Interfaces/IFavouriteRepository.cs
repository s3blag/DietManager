using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DM.Database;

namespace DM.Repositories.Interfaces
{
    public interface IFavouriteRepository: IBaseRepository<Favourite>
    {
        Task<IDictionary<Guid, int>> GetNumberOfFavouritesMarksAsync(IEnumerable<Guid> mealIds);
        Task<IEnumerable<Favourite>> GetUserFavouritesAsync(Guid userId);
        Task<bool> RemoveFromFavouritesAsync(Favourite favouriteToRemove);
    }
}