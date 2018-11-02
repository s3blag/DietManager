namespace DM.Models.Enums
{
    public enum ActivityType
    {
        NewMealAdded,
        NewMealIngredientAdded,
        PictureChanged
    }

    public enum FriendInvitationStatus
    {
        Accepted,
        Ignored,
        Awaiting
    }

#region Achievements
    public static class Achievements
    {
        public enum MealAchievement
        {
            NumberOfUses,
            NumberOfFavouriteMarks,
            AdditionsCountOver
        }

        public enum MealIngredientAchievement
        {
            AdditionsCountOver
        }

        public enum UserAchievement
        {
            Anniversary
        }

        public enum MealScheduleAchievement
        {
            ConsequentScheduleUpdates
        }

        public enum FriendAchievement
        {
            NumberOfFriends
        }
    }
    
    public enum AchievementCategory
    {
        Meal,
        MealIngredient,
        User,
        MealSchedule,
        Friends
    }

#endregion

}
