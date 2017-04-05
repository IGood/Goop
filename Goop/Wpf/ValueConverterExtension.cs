namespace Goop.Wpf
{
    using System;
    using System.Globalization;
    using System.Windows.Data;
    using System.Windows.Markup;

    [MarkupExtensionReturnType(typeof(IValueConverter))]
    public abstract class ValueConverterExtension<T> : MarkupExtension, IValueConverter where T : ValueConverterExtension<T>, new()
    {
        public static readonly T Default = new T();
        public override object ProvideValue(IServiceProvider serviceProvider) => Default;
        public virtual object Convert(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotSupportedException();
        public virtual object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotSupportedException();
    }
}
