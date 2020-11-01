using System;
using System.Collections.Generic;

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
    }
}
