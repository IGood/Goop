namespace Goop.Wpf
{
	using System;
	using System.Globalization;
	using System.Windows.Data;
	using System.Windows.Markup;

	[MarkupExtensionReturnType(typeof(IValueConverter))]
	public abstract class StatelessValueConverter<T> : MarkupExtension, IValueConverter where T : StatelessValueConverter<T>, new()
	{
		public static readonly T Default = new T();
		public sealed override object ProvideValue(IServiceProvider serviceProvider) => Default;
		public virtual object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture) => throw new NotSupportedException();
		public virtual object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) => throw new NotSupportedException();
	}
}
