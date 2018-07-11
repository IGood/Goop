namespace Goop.Wpf
{
    using System;
    using System.Windows.Markup;
    using System.Windows.Media;

    [MarkupExtensionReturnType(typeof(Color))]
    public class ColorExtension : MarkupExtension
    {
        public ColorExtension() { }
        public ColorExtension(Color color) => this.Color = color;
        public Color Color { get; set; }
        public override object ProvideValue(IServiceProvider serviceProvider) => this.Color;
    }
}
