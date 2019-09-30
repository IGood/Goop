namespace Goop.Wpf
{
    using System;
    using System.Windows.Markup;
    using System.Windows.Media;

    [MarkupExtensionReturnType(typeof(SolidColorBrush))]
    public class SolidColorBrushExtension : MarkupExtension
    {
        public SolidColorBrushExtension()
            : this(Colors.Transparent)
        {
        }

        public SolidColorBrushExtension(Color color)
        {
            this.Brush = new SolidColorBrush(color);
        }

        public SolidColorBrushExtension(Color color, bool freeze)
        {
            this.Brush = new SolidColorBrush(color);

            if (freeze)
            {
                this.Brush.Freeze();
            }
        }

        public SolidColorBrush Brush { get; set; }

        public override object ProvideValue(IServiceProvider serviceProvider) => this.Brush;
    }
}
