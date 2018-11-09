using System;
using System.Collections.Generic;

namespace DM.Models.ViewModels
{
    public class MealVM
    {
        public Guid Id { get; set; }

        public Guid? ImageId { get; set; }

        public string Name { get; set; }

        public int Calories { get; set; }

        public IEnumerable<MealIngredientWithQuantityVM> Ingredients { get; set; }
    }
}
