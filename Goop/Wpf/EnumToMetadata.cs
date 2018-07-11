namespace Goop.Wpf
{
    using Goop.ComponentModel;
    using System;
    using System.Globalization;
    using System.Windows;
    using System.Windows.Data;

    [ValueConversion(typeof(Enum), typeof(object))]
    public class EnumToMetadata : StatelessValueConverter<EnumToMetadata>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Enum enumValue)
            {
                var attrs = enumValue.GetAttributes<EnumMetadataAttribute>();
                foreach (var attr in attrs)
                {
                    if (parameter == null || object.Equals(attr.Key, parameter))
                    {
                        return attr.Metadata;
                    }
                }
            }

            return DependencyProperty.UnsetValue;
        }
    }
}
