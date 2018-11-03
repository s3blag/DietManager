using System.Collections.Generic;
using DM.Database;

namespace DM.Repositories.Interfaces
{
    public interface ICacheContainer
    {
        void Fill(IEnumerable<Achievement> achievements);
        Achievement Get(object achievement, int value);
    }
}