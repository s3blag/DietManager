namespace DM.Models.Enums
{
    public enum ActivityType
    {
        NewMealAdded,
        NewMealIngredientAdded,
        PictureChanged,
        AchievementReached
    }

    public enum FriendInvitationStatus
    {
        Accepted,
        Ignored,
        Awaiting
    }

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
}
