using System;

namespace DM.Models.Extensions
{
    public static class Extensions
    {
        public static int GetDifferenceInYears(this DateTimeOffset minuend, DateTimeOffset subtrahend) => ((int)(minuend - subtrahend).TotalDays) / 365;

        public static string FirstToLower(this string pascalCaseString) => char.ToLowerInvariant(pascalCaseString[0]) + pascalCaseString.Substring(1);
    }
}
