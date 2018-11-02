using System;
using System.Collections.Generic;
using DM.Database;

namespace DM.Repositories.Interfaces
{
    public interface IAchievementsContainer
    {
        void Fill(IEnumerable<Achievement> achievements);
        Achievement Get(object achievement, int value);
    }
}