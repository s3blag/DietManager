using System;

namespace DM.Models.Models
{
    public class UserAchievementCreation
    {
        public UserAchievementCreation(Guid achievementId, Guid userId)
        {
            AchievementId = achievementId;
            UserId = userId;
        }

        public Guid AchievementId { get; set; }
        public Guid UserId { get; set; }
        public bool Seen => false;
    }
}
