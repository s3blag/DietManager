﻿using System;
using System.ComponentModel.DataAnnotations;

namespace DM.Models.ViewModels
{
    public class UserActivityVM
    {
        [Required]
        public UserVM User { get; set; }

        #region Activities
        
        public MealPreviewVM Meal { get; set; }

        public MealIngredientVM MealIngredient { get; set; }

        public MealPreviewVM Favourite { get; set; }

        public UserVM Friend { get; set; }

        public AchievementVM Achievement { get; set; }

        #endregion

        [Required]
        public DateTimeOffset? ActivityDate { get; set; }
    }
}
