using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DM.Database;

namespace DM.Repositories.Interfaces
{
    public interface IUserRepository : IBaseRepository<User>
    {
        Task<User> GetUserAsync(Guid id);
        Task<IList<User>> GetUsersByQueryAsync(string query, int index, int takeAmount);
    }
}