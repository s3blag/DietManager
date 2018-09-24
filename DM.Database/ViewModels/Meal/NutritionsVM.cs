using System;

namespace DM.Models.ViewModels
{
    public class NutritionsVM
    {
        public float Protein { get; set; }
        public float Carbohydrates { get; set; }
        public float Fats { get; set; }
        public float? VitaminA { get; set; } 
        public float? VitaminC { get; set; } 
        public float? VitaminB6 { get; set; }
        public float? VitaminD { get; set; } 
    }
}