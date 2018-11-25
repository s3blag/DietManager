using System;

namespace DM.Models.ViewModels.User
{
    public class UserUpdateVM
    {
        public string Name { get; set; }

        public string Surname { get; set; }

        public string City { get; set; }

        public Guid? ImageId { get; set; }

        public string Password { get; set; }
    }
}
