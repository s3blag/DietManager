using System;

namespace DM.Models.ViewModels
{
    public class UserVM
    {
        public string Name { get; set; }

        public string Lastname { get; set; }

        public Guid? ImageId { get; set; }

        public Guid Id { get; set; }

        public int MutualFriendsCount { get; set; }

    }
}
