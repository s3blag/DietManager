using System;
using System.ComponentModel.DataAnnotations;

namespace DM.Models.ViewModels
{
    public class UserVM
    {
        [Required]
        public Guid? Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Surname { get; set; }

        public Guid? ImageId { get; set; }

        public int? MutualFriendsCount { get; set; }
    }
}
