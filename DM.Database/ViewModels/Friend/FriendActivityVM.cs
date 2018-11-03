using DM.Models.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace DM.Models.ViewModels
{
    public class FriendActivityVM
    {
        [Required]
        public Guid ContentId { get; set; }

        [Required]
        public Guid Id { get; set; }

        [Required]
        public UserVM User { get; set; }

        [Required]
        public ActivityType Activity { get; set; }

        [Required]
        public DateTimeOffset ActivityDate { get; set; }
    }
}
