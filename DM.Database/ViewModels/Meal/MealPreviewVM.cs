using System;
using System.ComponentModel.DataAnnotations;

namespace DM.Models.ViewModels
{
    public class MealPreviewVM
    {
        [Required]
        public Guid? Id { get; set; }

        public Guid? ImageId { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public int? Calories { get; set; }

        [Required]
        public int? NumberOfFavouriteMarks { get; set; }

        [Required]
        public int? NumberOfUses { get; set; }

        [Required]
        public DateTimeOffset? CreationDate { get; set; }

        public UserVM Creator { get; set; }

        public bool? IsFavourite { get; set; }
    }
}
