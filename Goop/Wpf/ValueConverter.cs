namespace Goop.Wpf
{
    using System;
    using System.Globalization;
    using System.Windows.Data;
    using System.Windows.Markup;

    [MarkupExtensionReturnType(typeof(IValueConverter))]
    public abstract class ValueConverter : MarkupExtension, IValueConverter
    {
        public sealed override object ProvideValue(IServiceProvider serviceProvider) => this;
        public virtual object Convert(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotSupportedException();
        public virtual object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotSupportedException();
    }
}
