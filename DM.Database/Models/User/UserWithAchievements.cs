using DM.Database;
using System.Collections.Generic;

namespace DM.Models
{
    public class UserWithAchievements
    {
        public User User { get; set; }

        public IEnumerable<UserAchievement> Achievements { get; set; }
    }
}
