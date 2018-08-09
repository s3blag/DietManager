using DM.Database;
using DM.Logic.Models;
using System;

namespace DM.Logic.Extensions
{
    public static class ToDbExtensions
    {
        public static User ToDb(this UserCreation userCreation)
        {
            return new User()
            {
                UserId = Guid.NewGuid(),
                Email = userCreation.Email,
                Password = userCreation.Password,
                UserName = userCreation.UserName
            };
        }
    }
}
