using System.Collections.Generic;

namespace Assets.Scripts.Core.Utils
{
    public static class RandomExtensions
    {
        public static T Random<T>(this IEnumerable<T> list, RandomProvider random)
        {
            var success = random.GetRandom(list, out T result);
            if (success)
                return result;
            return default;
        }
    }
}
