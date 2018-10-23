using System;
using System.Collections.Generic;
using System.Text;

namespace DM.Models.Models
{
    public class MealPreview
    {
        public MealPreview(Guid mealId, Guid? imageId, string mealName, int calories)
        {
            Id = mealId;
            ImageId = imageId;
            Name = mealName;
            Calories = calories;
        }

        public Guid Id { get; set; }
        public Guid? ImageId { get; set; }
        public string Name { get; set; }
        public int Calories { get; set; }
        public DateTimeOffset CreationDate { get; set; }
    }
}
