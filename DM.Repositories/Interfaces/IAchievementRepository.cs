using DM.Database;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DM.Repositories.Interfaces
{
    public interface IAchievementRepository: IBaseRepository<Achievement>
    {
        Task<IEnumerable<Achievement>> GetAll();
    }
}