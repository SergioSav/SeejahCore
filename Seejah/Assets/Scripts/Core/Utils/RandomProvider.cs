using System;

namespace Assets.Scripts.Core.Utils
{
    public class RandomProvider
    {
        private int _seed;
        private Random _generator;

        public RandomProvider(int seed)
        {
            _seed = seed;
            _generator = new Random(_seed);
        }

        /// <summary>
        /// Returns random int
        /// </summary>
        /// <param name="min"> included</param>
        /// <param name="max"> included</param>
        /// <param name="result"> min - max</param>
        /// <returns></returns>
        public bool GetRandom(int min, int max, out int result)
        {
            result = min;

            if (min > max)
                return false;

            result = _generator.Next(min, max + 1);
            return true;
        }

        public bool GetRandom<T>(out T result) where T : Enum
        {
            var values = Enum.GetValues(typeof(T));
            var index = _generator.Next(1, values.Length); // 0 - must be 'None' always
            result = (T) values.GetValue(index);
            return true;
        }
    }
}
