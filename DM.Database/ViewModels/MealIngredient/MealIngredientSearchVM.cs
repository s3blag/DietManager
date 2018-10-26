using System.ComponentModel.DataAnnotations;

namespace DM.Models.ViewModels
{
    public class MealIngredientSearchVM
    {   
        [Required]
        public string Query { get; set; }
    }
}
