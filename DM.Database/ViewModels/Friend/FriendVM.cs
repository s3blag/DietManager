using System;
using System.ComponentModel.DataAnnotations;

namespace DM.Models.ViewModels
{
    public class FriendVM
    {
        [Required]
        public Guid InvitedUserId { get; set; }

        [Required]
        public Guid InvitingUserId { get; set; }

        [Required]
        public bool Accepted { get; set; }
    }
}
