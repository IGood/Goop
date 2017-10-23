namespace Goop.Linq
{
    using System;
    using System.Collections.Generic;

    public static class EnumerableEx
    {
        public static int IndexOf<T>(this IEnumerable<T> source, T value, IEqualityComparer<T> comparer = null)
        {
            if (comparer == null)
            {
                comparer = EqualityComparer<T>.Default;
            }

            int i = 0;
            foreach (var item in source)
            {
                if (comparer.Equals(item, value))
                {
                    return i;
                }

                ++i;
            }

            return -1;
        }

        public static int IndexOf<T>(this IEnumerable<T> source, T value, IComparer<T> comparer)
        {
            if (comparer == null)
            {
                comparer = Comparer<T>.Default;
            }

            int i = 0;
            foreach (var item in source)
            {
                if (comparer.Compare(item, value) == 0)
                {
                    return i;
                }

                ++i;
            }

            return -1;
        }

        public static void ForEach<T, TResult>(this IEnumerable<T> source, Func<T, int, TResult> function)
        {
            int i = -1;
            foreach (var item in source)
            {
                function(item, ++i);
            }
        }

        public static void ForEach<T, TResult>(this IEnumerable<T> source, Func<T, TResult> function)
        {
            foreach (var item in source)
            {
                function(item);
            }
        }
    }
}
