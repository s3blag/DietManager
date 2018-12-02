using System;
using System.ComponentModel.DataAnnotations;

namespace DM.Models.ViewModels
{
    public class AwaitingFriendInvitationVM
    {
        public Guid? UserId { get; set; }

        public string Name { get; set; }

        public string Surname { get; set; }

        public Guid? ImageId { get; set; }

        public string City { get; set; }
    }
}
