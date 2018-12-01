using System;
using System.ComponentModel.DataAnnotations;

namespace DM.Models.ViewModels
{
    public class InvitationAction
    {
        [Required]
        public Guid? InvitingUserId { get; set; }
    }
}
