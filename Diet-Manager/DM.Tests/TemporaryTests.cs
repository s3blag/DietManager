using DM.Database;
using DM.Database.Repositories;
using LinqToDB.Data;
using System;
using System.Threading.Tasks;
using Xunit;

namespace DM.Tests
{
    public class TemporaryTests
    {
        [Fact]
        public async Task Test1()
        {
            DataConnection.DefaultSettings = new DBConnectionSettings();

            var userRepository = new UserRepository();

            var guid = new Guid("629fe72b-4bb0-4dc8-aa51-bfe9d65139a9");

            await userRepository.DeleteUserAsync(guid);

            await userRepository.AddUserAsync(new User()
            { 
                UserId = guid,
                Email = "121313",
                Password = "p@ssw0rd",
                UserName = "seblag"
            });

            var user = await userRepository.GetUserAsync(guid);
        }
    }
}
