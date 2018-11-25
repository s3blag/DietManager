using AutoMapper;
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

        public async Task<bool> DeleteAvatarAsync(Guid userId)
        {
            // get user and take his avatar id
            // check if avatar is not guid empty

            var user = await _userRepository.GetUserByIdAsync(userId);

            if (!user.ImageId.HasValue)
            {
                return false;
            }

            bool deleted =  await _imageService.DeleteImageAsync(user.ImageId.Value);

            if (deleted)
            {
                return await _userRepository.UpdateUserAvatar(userId, null);
            }

            return deleted;
        }

        public async Task<bool> UpsertAvatarAsync(Guid userId, Guid newAvatarId)
        {
            var user = await _userRepository.GetUserByIdAsync(userId);

            var currentAvatarId = user.ImageId;

            if (currentAvatarId != null)
            {
                bool previousAvatarDeleted = false;

                previousAvatarDeleted =  await _imageService.DeleteImageAsync(currentAvatarId.Value);

                if (!previousAvatarDeleted)
                {
                    bool newAvatarDeleted = await _imageService.DeleteImageAsync(currentAvatarId.Value);
                   
                    if(!newAvatarDeleted)
                    {
                        throw new Exception("Could not delete both new and previous avatars");
                    }

                    throw new Exception("Could not delete previous avatar");
                }
            }

            return await _userRepository.UpdateUserAvatar(userId, newAvatarId);
        }

        public async Task<LoggedInUserVM> GetUserInfoAsync(Guid userId)
        {
            return _mapper.Map<LoggedInUserVM>(await _userRepository.GetUserByIdAsync(userId));
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

            return true;
        }
    }
}
