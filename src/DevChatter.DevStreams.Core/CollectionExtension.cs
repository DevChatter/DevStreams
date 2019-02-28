using System;
using System.Collections.Generic;
using System.Text;

namespace DevChatter.DevStreams.Core
{
    public static class CollectionExtension
    {
        private static Random _random = new Random();

        /// <summary>
        /// Pull random items from set
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="items"></param>
        /// <param name="count">number of unique items to pull</param>
        /// <returns></returns>
        public static List<T> PickRandom<T>(this List<T> items, int count)
        {
            var result = new List<T>();
            int index = _random.Next(0, items.Count);
            for (int i = 0; i < count; i++)
            {
                int moddedIndex = (index + i) % items.Count;
                result.Add(items[moddedIndex]);
            }
            return result;
        }

        public static T PickOneRandomElement<T>(this IList<T> list)
        {
            return list[_random.Next(list.Count)];
        }
    }
}
