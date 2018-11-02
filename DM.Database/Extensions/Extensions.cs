using System;

namespace DM.Models.Extensions
{
    public static class Extensions
    {
        public static int GetDifferenceInYears(this DateTimeOffset minuend, DateTimeOffset subtrahend) => ((int)(minuend - subtrahend).TotalDays) / 365;
    }
}
