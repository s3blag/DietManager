using System;
using System.ComponentModel.DataAnnotations;

namespace DM.Models.ViewModels
{
    public class FavouriteVM
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        public UserVM User { get; set; }

        [Required]
        public MealVM Meal { get; set; }
    }
}
