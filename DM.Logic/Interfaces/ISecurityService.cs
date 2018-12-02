using DM.Models.ViewModels;

namespace DM.Logic.Interfaces
{
    public interface ISecurityService
    {
        AuthToken GenerateAuthToken(UserVM user);
        bool VerifyPassword(string password, string encryptedPassword);
        string EncryptPassword(string password);
    }
}