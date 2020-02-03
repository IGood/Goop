using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Goop.Linq
{
	public static class EnumerableEx
	{
		public static IEnumerable<T> Once<T> (this T value)
		{
			yield return value;
		}

		public static int IndexOf<T> (this IEnumerable<T> source, T value, IEqualityComparer<T>? comparer = null)
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

		public static int IndexOf<T> (this IEnumerable<T> source, T value, IComparer<T>? comparer)
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

		public static int FindIndex<T> (this IEnumerable<T> source, Predicate<T> match)
		{
			int i = 0;
			foreach (var item in source)
			{
				if (match(item))
				{
					return i;
				}

				++i;
			}

			return -1;
		}

		public static void ForEach<T> (this IEnumerable<T> source, Action<T, int> action)
		{
			int i = -1;
			foreach (var item in source)
			{
				action(item, ++i);
			}
		}

		public static void ForEach<T> (this IEnumerable<T> source, Action<T> action)
		{
			foreach (var item in source)
			{
				action(item);
			}
		}

		public static void ForEach<T, TResult> (this IEnumerable<T> source, Func<T, int, TResult> function)
		{
			int i = -1;
			foreach (var item in source)
			{
				function(item, ++i);
			}
		}

		public static void ForEach<T, TResult> (this IEnumerable<T> source, Func<T, TResult> function)
		{
			foreach (var item in source)
			{
				function(item);
			}
		}

		public static void ForEachR<T> (this IList<T> source, Action<T> action)
		{
			int i = source.Count;
			while (--i >= 0)
			{
				action(source[i]);
			}
		}

		public static IEnumerable<string?> DistinctI (this IEnumerable<string?> source)
		{
			return source.Distinct(StringComparer.OrdinalIgnoreCase);
		}

		public static IEnumerable<TSource> DistinctBy<TSource, TKey> (this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, IEqualityComparer<TKey>? comparer = null)
			where TKey : notnull
		{
			var keys = new HashSet<TKey>(comparer);

			foreach (var item in source)
			{
				if (keys.Add(keySelector(item)))
				{
					yield return item;
				}
			}
		}

		public static IEnumerable<TSource> DistinctByI<TSource> (this IEnumerable<TSource> source, Func<TSource, string> keySelector)
		{
			var keys = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

			foreach (var item in source)
			{
				if (keys.Add(keySelector(item)))
				{
					yield return item;
				}
			}
		}

		public static TResult[] ToArray<TSource, TResult> (this ICollection<TSource> source, Converter<TSource, TResult> selector)
		{
			var result = new TResult[source.Count];

			int i = -1;
			foreach (var item in source)
			{
				result[++i] = selector(item);
			}

			return result;
		}

		public static Dictionary<string, T> ToDictionaryI<T> (this IEnumerable<T> source, Func<T, string> keySelector)
		{
			return source.ToDictionary(keySelector, StringComparer.OrdinalIgnoreCase);
		}

		public static Dictionary<string, TElement> ToDictionaryI<TSource, TElement> (this IEnumerable<TSource> source, Func<TSource, string> keySelector, Func<TSource, TElement> elementSelector)
		{
			return source.ToDictionary(keySelector, elementSelector, StringComparer.OrdinalIgnoreCase);
		}

		public static HashSet<T> ToHashSet<T> (this IEnumerable<T> source, IEqualityComparer<T>? comparer = null)
		{
			return new HashSet<T>(source, comparer);
		}

		public static HashSet<string> ToHashSetI (this IEnumerable<string> source)
		{
			return new HashSet<string>(source, StringComparer.OrdinalIgnoreCase);
		}

		public static ObservableCollection<T> ToObservableCollection<T> (this IEnumerable<T> source)
		{
			return new ObservableCollection<T>(source);
		}

		public static int BinarySearch<T> (this IList<T> list, T value, IComparer<T>? comparer = null)
		{
			if (comparer == null)
			{
				comparer = Comparer<T>.Default;
			}

			int min = 0;
			int max = list.Count - 1;

			while (min <= max)
			{
				int current = min + (max - min) / 2;
				int result = comparer.Compare(value, list[current]);

				if (result == 0)
				{
					return current;
				}

				if (result < 0)
				{
					max = current - 1;
				}
				else
				{
					min = current + 1;
				}
			}

			return ~min;
		}
	}
}
