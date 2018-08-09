using DM.Database.Repositories;
using DM.Logic.Extensions;
using DM.Logic.Models;
using DM.Logic.Models.User;
using System;
using System.Threading.Tasks;

namespace DM.Logic.Services
{
    public class UserService
    {
        private readonly UserRepository _userRepository;

        public UserService(UserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<Guid> AddUserAsync(UserCreation userCreation)
        {
            if (userCreation == null)
            {
                throw new ArgumentNullException(nameof(userCreation));
            }

            // Check whether email is unique
            var userDb = userCreation.ToDb();

            await _userRepository.AddUserAsync(userDb);

            return userDb.UserId;
        }

        public async Task DeleteUserAsync(Guid guid)
        {
            if (guid == default(Guid))
            {
                throw new ArgumentNullException(nameof(guid));
            }

            await _userRepository.DeleteUserAsync(guid);
        }

        public async Task<User> GetUserByGuidAsync(Guid guid)
        {
            if (guid == default(Guid))
            {
                throw new ArgumentNullException(nameof(guid));
            }

            var dbUser = await _userRepository.GetUserAsync(guid);

            return new User(dbUser);
        }
    }
}
