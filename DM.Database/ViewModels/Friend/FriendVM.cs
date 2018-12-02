using System;
using System.ComponentModel.DataAnnotations;

namespace DM.Models.ViewModels
{
    public class FriendVM
    {
        public Guid? InvitedUserId { get; set; }

        public Guid? InvitingUserId { get; set; }

        public bool? Accepted { get; set; }
    }
}
