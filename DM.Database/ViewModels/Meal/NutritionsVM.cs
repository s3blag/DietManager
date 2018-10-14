using System;
using System.ComponentModel.DataAnnotations;

namespace DM.Models.ViewModels
{
    public class NutritionsVM
    {
        [Required]
        [Range(0, float.MaxValue)]
        public float Protein { get; set; }
        [Required]
        [Range(0, float.MaxValue)]
        public float Carbohydrates { get; set; }
        [Required]
        [Range(0, float.MaxValue)]
        public float Fats { get; set; }
        public float? VitaminA { get; set; } 
        public float? VitaminC { get; set; } 
        public float? VitaminB6 { get; set; }
        public float? VitaminD { get; set; } 
    }
}