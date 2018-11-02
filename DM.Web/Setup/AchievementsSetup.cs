using DM.Database;
using DM.Models.Config;
using DM.Models.Exceptions;
using DM.Repositories.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public static class AchievementsSetup
{
    public static async Task SetupAchievements(AchievementsConfig config, IAchievementRepository achievementRepository)
    {
        var dbAchievements = await GetAchievementsFromDb(achievementRepository);

        var achievementsFromConfig = GetAchievementsFromConfig(config);

        if (!dbAchievements.Any())
        {
            await AddAchievementsAsync(achievementsFromConfig, achievementRepository);
        }
        else
        {
            if (dbAchievements.Count() != achievementsFromConfig.Count)
            {
                throw new DataAccessException("Achievements setup failed - different number of achievements");
            }
        }
    }

    private static async Task<IEnumerable<Achievement>> GetAchievementsFromDb(IAchievementRepository repository) => await repository.GetAllAsync();

    private static async Task AddAchievementsAsync(IEnumerable<Achievement> achievements, IAchievementRepository achievementsRepository)
    {
        if (!await achievementsRepository.AddAchievementsAsync(achievements))
        {
            throw new DataAccessException("Achievements setup failed - adding achievements to the DB failed");
        } 
    }

    private static IList<Achievement> GetAchievementsFromConfig(AchievementsConfig instance)
    {
        var achievements = new List<Achievement>();

        var properties = instance.GetType().GetProperties();

        foreach (var property in properties)
        {
            var dictionary = property.GetValue(instance) as IDictionary;
            if (dictionary != null)
            {
                foreach (DictionaryEntry keyValuePair in dictionary)
                {
                    achievements.AddRange(
                        ((int[])keyValuePair.Value).
                            Select(_ => new Achievement()
                            {
                                Id = Guid.NewGuid(),
                                Category = keyValuePair.Key.GetType().Name,
                                Type = keyValuePair.Key.ToString(),
                                Value = _
                            }));
                }
            }
        }

        return achievements;
    }
}
