using System.Collections.Generic;

namespace DM.Models.ViewModels
{
    public class GroupedUserAchievementsVM
    {
        public Dictionary<string, Dictionary<string, IEnumerable<int>>> GroupedAchievements { get; set; }

        public bool Any { get; set; }
    }
}
