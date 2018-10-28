using DM.Models.Config;
using Microsoft.Extensions.Options;

namespace DM.Logic.Services
{
    public class AchievementService
    {
        private readonly AchievementsConfig _config;

        public AchievementService(IOptions<AchievementsConfig> options)
        {
            _config = options.Value;
        }



    }
}