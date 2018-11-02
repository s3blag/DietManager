using System.ComponentModel.DataAnnotations;

namespace DM.Models.ViewModels
{
    public class AchievementVM
    {
        [Required]
        public string Category { get; set; }
        [Required]
        public string Type { get; set; }
        [Required]
        public int Value { get; set; }

    }
}
