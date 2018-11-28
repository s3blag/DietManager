namespace DM.Models.ViewModels
{
    public class UserWithAchievementsVM
    {
        public UserWithAchievementsVM(GroupedUserAchievementsVM achievements, UserVM user)
        {
            Achievements = achievements;
            User = user;
        }

        public GroupedUserAchievementsVM Achievements { get; set; }

        public UserVM User { get; set; }
    }
}
