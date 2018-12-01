using DM.Models.ViewModels;

namespace DM.Logic.Interfaces
{
    public interface IAuthService
    {
        AuthToken GenerateAuthToken(UserVM user);
    }
}