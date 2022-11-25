using System;
using System.Collections.Generic;
using System.Linq;

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
        /// Random int
        /// </summary>
        /// <param name="min"> included</param>
        /// <param name="max"> included</param>
        /// <param name="result"> min - max</param>
        /// <returns>random int</returns>
        public bool GetRandom(int min, int max, out int result)
        {
            result = min;

            if (min > max)
                return false;

            result = _generator.Next(min, max + 1);
            return true;
        }

        /// <summary>
        /// Random item from enum
        /// </summary>
        /// <typeparam name="T">Enum</typeparam>
        /// <param name="result"></param>
        /// <returns>random item from enum</returns>
        public bool GetRandom<T>(out T result) where T : Enum
        {
            var values = Enum.GetValues(typeof(T));
            var index = _generator.Next(1, values.Length); // 0 - must be 'None' always
            result = (T) values.GetValue(index);
            return true;
        }

        /// <summary>
        /// Random item from list
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="result"></param>
        /// <returns>random item from list</returns>
        public bool GetRandom<T>(List<T> list, out T result)
        {
            result = default;
            if (list == null || list.Count == 0)
                return false;

            var index = _generator.Next(0, list.Count);
            result = list[index];
            return true;
        }

        /// <summary>
        /// Random item from IEnumerable
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="result"></param>
        /// <returns>random item from IEnumerable</returns>
        public bool GetRandom<T>(IEnumerable<T> list, out T result)
        {
            result = default;
            if (list == null)
                return false;
            var listLength = list.Count();
            if (listLength == 0) 
                return false;

            var index = _generator.Next(0, listLength);
            result = list.ElementAt(index);
            return true;
        }
    }
}
