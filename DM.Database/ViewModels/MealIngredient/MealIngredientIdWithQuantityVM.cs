using System;
using System.ComponentModel.DataAnnotations;

namespace DM.Models.ViewModels
{
    public class MealIngredientIdWithQuantityVM
    {
        [Required]
        public Guid? Id { get; set; }

        [Required]
        public double? Quantity { get; set; }
    }
}
