﻿namespace Goop.Wpf
{
    using System;
    using System.Globalization;
    using System.Windows.Data;
    using System.Windows.Markup;

    [MarkupExtensionReturnType(typeof(IMultiValueConverter))]
    public abstract class MultiValueConverterExtension<T> : MarkupExtension, IMultiValueConverter where T : new()
    {
        public static readonly T Default = new T();
        public override object ProvideValue(IServiceProvider serviceProvider) => Default;
        public virtual object Convert(object[] values, Type targetType, object parameter, CultureInfo culture) => throw new NotSupportedException();
        public virtual object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture) => throw new NotSupportedException();
    }
}
