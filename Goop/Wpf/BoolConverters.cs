namespace Goop.Wpf
{
	using System;
	using System.Globalization;
	using System.Windows;
	using System.Windows.Data;
	using System.Windows.Markup;

	[ValueConversion(typeof(bool), typeof(object))]
	[MarkupExtensionReturnType(typeof(BoolConverter))]
	public class BoolConverter : ValueConverter
	{
		public object? IfTrue { get; set; }

		public object? Else { get; set; }

		public override object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
		{
			return true.Equals(value) ? this.IfTrue : this.Else;
		}

		public override object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
		{
			return Equals(value, this.IfTrue);
		}
	}

	[ValueConversion(typeof(bool), typeof(Visibility))]
	[MarkupExtensionReturnType(typeof(BoolToVisibilityConverter))]
	public class BoolToVisibilityConverter : BoolConverter
	{
		public BoolToVisibilityConverter()
		{
			this.IfTrue = Visibility.Visible;
			this.Else = Visibility.Collapsed;
		}
	}
}
