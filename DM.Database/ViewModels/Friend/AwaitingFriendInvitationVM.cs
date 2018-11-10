using System;
using System.ComponentModel.DataAnnotations;

namespace DM.Models.ViewModels
{
    public class AwaitingFriendInvitationVM
    {
        [Required]
        public Guid? UserId { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Surname { get; set; }

        public Guid? ImageId { get; set; }
    }
}
