using DM.Models.ViewModels;
using System;
using System.Threading.Tasks;

namespace DM.Logic.Interfaces
{
    public interface IUserService
    {
        Task<UserVM> CreateUserAsync(UserCreationVM userCreation);
        Task<UserWithPasswordVM> GetUserByLoginDataAsync(LoginVM login);
        Task<bool> DeleteAvatarAsync(Guid userId);
        Task<bool> UpsertAvatarAsync(Guid userId, Guid newAvatarId);
        Task<UserVM> GetUserInfoAsync(Guid userId);
        Task<bool> DeleteAccountAsync(Guid userId);
    }
}