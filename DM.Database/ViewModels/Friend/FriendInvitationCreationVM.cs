using System;
using System.ComponentModel.DataAnnotations;

namespace DM.Models.ViewModels
{
    public class FriendInvitationCreationVM
    {
        
        public Guid? InvitingUserId { get; set; }

        [Required]
        public Guid? InvitedUserId { get; set; }

    }
}
