namespace Goop.Wpf
{
    using System;
    using System.Globalization;
    using System.Windows.Data;
    using System.Windows.Markup;

    [MarkupExtensionReturnType(typeof(IMultiValueConverter))]
    public abstract class MultiValueConverter : MarkupExtension, IMultiValueConverter
    {
        public sealed override object ProvideValue(IServiceProvider serviceProvider) => this;
        public virtual object Convert(object[] values, Type targetType, object parameter, CultureInfo culture) => throw new NotSupportedException();
        public virtual object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture) => throw new NotSupportedException();
    }
}
