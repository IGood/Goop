using System;
using System.Text;

namespace Goop
{
	public static class StringBuilderEx
	{
		public static StringBuilder AppendFormatted (this StringBuilder builder, FormattableString value)
		{
			builder.AppendFormat(value.Format, value.GetArguments());
			return builder;
		}
	}
}
