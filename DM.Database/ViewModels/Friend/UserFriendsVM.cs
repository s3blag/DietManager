using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DM.Models.ViewModels
{
    public class UserFriendsVM
    {
        [Required]
        public Guid? UserId { get; set; }

        [Required]
        public IEnumerable<UserVM> Friends { get; set; }
    }
}
