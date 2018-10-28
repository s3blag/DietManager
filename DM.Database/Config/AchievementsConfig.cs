using DM.Models.Enums;
using System;
using System.Collections.Generic;
using static DM.Models.Enums.Achievements;

namespace DM.Models.Config
{
    public class AchievementsConfig
    {
        public Dictionary<MealAchievement, int[]> MealAchievements { get; set; }

        public Dictionary<MealIngredientAchievement, int[]> MealIngredientAchievements { get; set; }

        public Dictionary<UserAchievement, int[]> UserAchievements { get; set; }

        public Dictionary<MealScheduleAchievement, int[]> MealScheduleAchievements { get; set; }

        public Dictionary<FriendAchievement, int[]> FriendAchievements { get; set; }
    }
}
