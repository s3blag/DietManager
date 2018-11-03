using DM.Database;
using DM.Repositories.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace DM.Repositories
{
    public class AchievementsCacheContainer : ICacheContainer
    {
        private readonly List<Achievement> _achievements = new List<Achievement>();
        
        public void Fill(IEnumerable<Achievement> achievements)
        {
            _achievements.Clear();
            _achievements.AddRange(achievements);
        }

        public Achievement Get(object achievement, int value)
        {
            return _achievements.
                Where(a => a.Category == achievement.GetType().ToString()).
                Where(a => a.Type == achievement.ToString()).
                Where(a => a.Value == value).
                FirstOrDefault();             
        }
    }
}
