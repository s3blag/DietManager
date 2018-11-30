using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DM.Models.ViewModels
{
    public class MealVM
    {
        [Required]
        public Guid? Id { get; set; }

        public Guid? ImageId { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public int? Calories { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public int NumberOfUses { get; set; }

        [Required]
        public int NumberOfFavouriteMarks { get; set; } = 0;

        [Required]
        public UserVM Creator { get; set; }

        [Required]
        public bool? IsFavourite { get; set; }

        [Required]
        [MinLength(2)]
        public IEnumerable<MealIngredientWithQuantityVM> Ingredients { get; set; }
    }
}
