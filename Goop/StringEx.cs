using System;

namespace Goop
{
	public static class StringEx
	{
		public static int CompareToI (this string value, string? other)
		{
			return string.Compare(value, other, StringComparison.OrdinalIgnoreCase);
		}

		public static bool ContainsI (this string value, string other)
		{
			return value.Contains(other, StringComparison.OrdinalIgnoreCase);
		}

		public static bool EqualsI (this string value, string? other)
		{
			return value.Equals(other, StringComparison.OrdinalIgnoreCase);
		}
	}
}
