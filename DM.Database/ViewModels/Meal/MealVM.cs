using System;
using System.Collections.Generic;

namespace DM.Models.ViewModels
{
    public class MealVM
    {
        public Guid Id { get; set; }

        public Guid? PhotoId { get; set; }

        public string Name { get; set; }

        public string Calories { get; set; }

        public IEnumerable<MealIngredientVM> Ingredients { get; set; }
    }
}
