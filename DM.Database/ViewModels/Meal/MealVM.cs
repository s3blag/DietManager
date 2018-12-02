using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DM.Models.ViewModels
{
    public class MealVM
    {
        public Guid? Id { get; set; }

        public Guid? ImageId { get; set; }

        public string Name { get; set; }

        public int? Calories { get; set; }

        public string Description { get; set; }

        public int NumberOfUses { get; set; }

        public int NumberOfFavouriteMarks { get; set; } = 0;

        public UserVM Creator { get; set; }

        public bool? IsFavourite { get; set; }

        public IEnumerable<MealIngredientWithQuantityVM> Ingredients { get; set; }
    }
}
