using System;
using System.Collections.Generic;
using System.Linq;

namespace TabataGeneratorTests.Helpers
{
    public static class LinqHelperForTests
    {
        public static IEnumerable<(T, T)> SelectTwoConsecutives<T>(this IEnumerable<T> source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            return SelectTwoImpl(source);
        }

        private static IEnumerable<(T, T)> SelectTwoImpl<T>(this IEnumerable<T> source)
        {
            using var iterator = source.GetEnumerator();
            var item2 = default(T);
            var i = 0;
            while (iterator.MoveNext())
            {
                var item1 = item2;
                item2 = iterator.Current;
                i++;

                if (i >= 2)
                {
                    yield return (item1, item2);
                }
            }
        }

        internal static (T, T) AsTupple2<T>(this IEnumerable<T> enumerable) => (enumerable.ElementAt(0), enumerable.ElementAt(1));
        internal static (T, T, T) AsTupple3<T>(this IEnumerable<T> enumerable) => (enumerable.ElementAt(0), enumerable.ElementAt(1), enumerable.ElementAt(2));
    }
}
