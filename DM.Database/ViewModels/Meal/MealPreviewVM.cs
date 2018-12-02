using System;
using System.ComponentModel.DataAnnotations;

namespace DM.Models.ViewModels
{
    public class MealPreviewVM
    {
        public Guid? Id { get; set; }

        public Guid? ImageId { get; set; }

        public string Name { get; set; }

        public int? Calories { get; set; }

        public int? NumberOfFavouriteMarks { get; set; }

        public int? NumberOfUses { get; set; }

        public DateTimeOffset? CreationDate { get; set; }

        public UserVM Creator { get; set; }

        public bool? IsFavourite { get; set; }
    }
}
