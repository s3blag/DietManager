using System;
using System.ComponentModel.DataAnnotations;

namespace DM.Models.ViewModels
{

    public class UserAchievementVM
    {
        [Required]
        public Guid? Id { get; set; }

        [Required]
        public string Category { get; set; }

        [Required]
        public string Type { get; set; }

        [Required]
        public int Value { get; set; }

        public bool? Seen { get; set; }
    }
}
