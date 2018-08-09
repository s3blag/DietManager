using System;
using System.Collections.Generic;
using System.Text;

namespace DM.Logic.Models.User
{
    public class User
    {
        public User(Database.User user)
        {
            Id = user.UserId;
            UserName = user.UserName;
            Email = user.Email;
        }

        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
    }
}
