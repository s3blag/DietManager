using DM.Database;
using System;
using System.Collections.Generic;
using System.Text;

namespace DM.Models.ViewModels
{
    public class MealVM
    {
        public Guid Id { get; set; }

        public Guid PhotoId { get; set; }

        public string Name { get; set; }

        public string Calories { get; set; }

        public IEnumerable<MealIngredientVM> Ingredients { get; set; }
    }
}
