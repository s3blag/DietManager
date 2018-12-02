using AutoMapper;
using DM.Database;
using DM.Logic.Interfaces;
using DM.Models.ViewModels;
using DM.Repositories.Interfaces;
using System;
using System.Threading.Tasks;

namespace DM.Logic.Services
{
    public class UserService : IUserService
    {
        private readonly IImageService _imageService;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UserService(IImageService imageService, IUserRepository userRepository, IMapper mapper)
        {
            _imageService = imageService;
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<UserVM> CreateUserAsync(UserCreationVM userCreation)
        {
            bool isUsernameUnique = await _userRepository.IsUsernameUniqueAsync(userCreation.Username);

            if (!isUsernameUnique)
            {
                return null;
            }

            var dbUser = _mapper.Map<User>(userCreation);

            bool userAdded = await _userRepository.AddAsync(dbUser);

            if (userAdded)
            {
                return _mapper.Map<UserVM>(dbUser);
            }

            return null;  
        }

        public async Task<UserWithPasswordVM> GetUserByLoginDataAsync(LoginVM login)
        {
            return _mapper.Map<UserWithPasswordVM>(
                await _userRepository.GetUserByLoginDataAsync(login.Username)
                );
        }

        public async Task<bool> DeleteAvatarAsync(Guid userId)
        {
            var user = await _userRepository.GetUserByIdAsync(userId);

            if (!user.ImageId.HasValue)
            {
                return false;
            }

            await _userRepository.UpdateUserAvatar(userId, null);

            await _imageService.DeleteImageAsync(user.ImageId.Value);

            return true;
        }

        public async Task<bool> UpsertAvatarAsync(Guid userId, Guid newAvatarId)
        {
            var user = await _userRepository.GetUserByIdAsync(userId);

            var oldAvatarId = user.ImageId;

            await _userRepository.UpdateUserAvatar(userId, newAvatarId);

            if (oldAvatarId != null)
            {
                bool previousAvatarDeleted = false;

                previousAvatarDeleted =  await _imageService.DeleteImageAsync(oldAvatarId.Value);

                if (!previousAvatarDeleted)
                {
                    bool newAvatarDeleted = await _imageService.DeleteImageAsync(oldAvatarId.Value);
                   
                    if(!newAvatarDeleted)
                    {
                        throw new Exception("Could not delete both new and previous avatars");
                    }

                    throw new Exception("Could not delete previous avatar");
                }
            }

            return true;
        }

        public async Task<UserVM> GetUserInfoAsync(Guid userId)
        {
            return _mapper.Map<UserVM>(await _userRepository.GetUserByIdAsync(userId));
        }

        public async Task<bool> DeleteAccountAsync(Guid userId)
        {
            var user = await _userRepository.GetUserByIdAsync(userId);

            if (user.ImageId != null)
            {
                bool imagedeleted = await _imageService.DeleteImageAsync(user.ImageId.Value);

                if (!imagedeleted)
                {
                    return false;
                }
            }

            await _userRepository.DeleteUserRelatedDataAsync(user);
            
            return await _userRepository.DeleteAsync(user);
        }
    }
}
