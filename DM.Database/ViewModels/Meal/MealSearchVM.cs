using System.ComponentModel.DataAnnotations;

namespace DM.Models.ViewModels
{
    public class MealSearchVM
    {
        [Required]
        public string Query { get; set; }
    }
}
