using DM.Models.ViewModels;
using System;
using System.Threading.Tasks;

namespace DM.Logic.Interfaces
{
    public interface IUserService
    {
        Task<UserVM> GetUserByLoginDataAsync(LoginVM login);
        Task<bool> DeleteAvatarAsync(Guid userId);
        Task<bool> UpsertAvatarAsync(Guid userId, Guid newAvatarId);
        Task<LoggedInUserVM> GetUserInfoAsync(Guid userId);
        Task<bool> DeleteAccountAsync(Guid userId);
    }
}