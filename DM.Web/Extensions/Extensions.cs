using DM.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DM.Web.Extensions
{
    public static class Extensions
    {
        public static Dictionary<string, K> ParseEnumToLower<T, K>(this Dictionary<T, K> dictionary)
        {
            return dictionary.ToDictionary(kv => kv.Key.ToString().ToLower(), kv => kv.Value);
        }
    }
}
