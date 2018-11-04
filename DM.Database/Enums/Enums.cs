namespace DM.Models.Enums
{
    public enum ActivityType
    {
        NewMealAdded,
        NewMealIngredientAdded,
        AchievementReached,
        NewFriend,
        AddToFavourites,
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
