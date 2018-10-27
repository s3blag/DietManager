using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DM.Database;

namespace DM.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task AddUserAsync(User user);
        Task DeleteUserAsync(Guid id);
        Task<User> GetUserAsync(Guid id);
        Task<IList<User>> GetUsersByQueryAsync(string query, int index, int takeAmount);
    }
}