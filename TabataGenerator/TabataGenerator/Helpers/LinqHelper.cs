using System;
using System.Collections.Generic;

namespace TabataGenerator.Helpers
{
    public static class LinqHelper
    {
        public static IEnumerable<T> Sandwich<T>(this IEnumerable<T> enumerable, T lettuce)
        {
            using var enumerator = enumerable.GetEnumerator();
            if (enumerator.MoveNext())
            {
                yield return enumerator.Current;
            }
            else
            {
                yield break;
            }

            while (enumerator.MoveNext())
            {
                yield return lettuce;
                yield return enumerator.Current;
            }
        }

        public static void ForEach(in int count, Action<int, bool, bool> action)
        {
            for (var i = 0; i < count; i++)
            {
                var first = i == 0;
                var last = i == count - 1;
                action(i, first, last);
            }
        }

        public static void ForEach<T>(T[] items, Action<int, T, bool, bool> action)
        {
            for (var index = 0; index < items.Length; index++)
            {
                var item = items[index];
                var first = index == 0;
                var last = index == items.Length - 1;
                action(index, item, first, last);
            }
        }

        public static string Concat(this IEnumerable<string> enumerable)
            => string.Concat(enumerable);
    }
}
