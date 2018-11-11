using System;
using System.ComponentModel.DataAnnotations;

namespace DM.Models.ViewModels
{
    public class FavouriteCreationVM
    {
        public Guid? UserId { get; set; }

        [Required]
        public Guid? MealId { get; set; }
    }
}
