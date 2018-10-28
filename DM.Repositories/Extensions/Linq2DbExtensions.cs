using LinqToDB;
using System;

namespace DM.Repositories.Extensions
{
    public static class Linq2DbExtensions
    {
        [Sql.Expression("{0} > {1}", PreferServerSide = true, IsPredicate = true)]
        public static bool GreaterThan<T>(this T x, T toCompare) where T : IComparable<T> => x.CompareTo(toCompare) > 0;

        [Sql.Expression("{0} < {1}", PreferServerSide = true, IsPredicate = true)]
        public static bool LessThan<T>(this T x, T toCompare) where T : IComparable<T> => x.CompareTo(toCompare) < 0;
    
        [Sql.Expression("{0} >= {1}", PreferServerSide = true, IsPredicate = true)]
        public static bool GreaterThanOrEqual<T>(this T x, T toCompare) where T : IComparable<T> { return x.CompareTo(toCompare) >= 0; }

        [Sql.Expression("{0} <= {1}", PreferServerSide = true, IsPredicate = true)]
        public static bool LessThanOrEqual<T>(this T x, T toCompare) where T : IComparable<T> => x.CompareTo(toCompare) <= 0;

        [Sql.Expression("(EXTRACT(epoch from age({0}, {1})) / 86400)::int", PreferServerSide = true, IsPredicate = true)]
        public static int SubtractWithResultInDays(this DateTimeOffset startDate, DateTimeOffset endDate) 
            => (int)((startDate- endDate).TotalDays);
    }
}
