using DM.Database;
using System;

namespace DM.Models.Models
{
    public class MealPreview
    {
        public MealPreview() {}

        public MealPreview(Guid mealId, User creator, Guid? imageId, string mealName, int calories, int numberOfUses, int numberOfFavouriteMarks, DateTimeOffset creationDate)
        {
            Id = mealId;
            Creator = creator;
            ImageId = imageId;
            Name = mealName;
            Calories = calories;
            CreationDate = creationDate;
            NumberOfFavouriteMarks = numberOfFavouriteMarks;
            NumberOfUses = numberOfUses;
        }

        public Guid Id { get; set; }
        public Guid? ImageId { get; set; }
        public string Name { get; set; }
        public int Calories { get; set; }
        public int NumberOfUses { get; set; }
        public int NumberOfFavouriteMarks { get; set; } = 0;
        public DateTimeOffset CreationDate { get; set; }
        public User Creator { get; set; }
    }
}
