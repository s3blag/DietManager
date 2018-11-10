using System;
using System.ComponentModel.DataAnnotations;

namespace DM.Models.ViewModels
{

    public class UserAchievementVM
    {
        [Required]
        public Guid? Id { get; set; }
        [Required]
        public AchievementVM Achievement { get; set; }
        [Required]
        public bool? Seen { get; set; }
    }
}
