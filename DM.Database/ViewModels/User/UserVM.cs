using System;
using System.ComponentModel.DataAnnotations;

namespace DM.Models.ViewModels
{
    public class UserVM
    {
        [Required]
        public Guid? Id { get; set; }

        public Guid? ImageId { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Surname { get; set; }

        public string City { get; set; }

        public int? MutualFriendsCount { get; set; }
    }
}
