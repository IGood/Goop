using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;

namespace Goop.Wpf.IconGlyph
{
	using DP = DependencyPropertyUtilities<IconGlyphTextBlock>;

	[ContentProperty(nameof(IconGlyph))]
	public class IconGlyphTextBlock : TextBlock
	{
		static IconGlyphTextBlock ()
		{
			DP.OverrideMetadata(
				TextBlock.FontFamilyProperty,
				new FrameworkPropertyMetadata(new FontFamily("Segoe MDL2 Assets")) { Inherits = false });

			DP.OverrideMetadata(TextBlock.FontSizeProperty, new FrameworkPropertyMetadata(16.0));
		}

		public static readonly DependencyProperty IconGlyphProperty = DP.Register(_ => _.IconGlyph, IconGlyph.Unknown, IconGlyphPropertyChanged);

		public IconGlyph IconGlyph
		{
			get => (IconGlyph)this.GetValue(IconGlyphProperty);
			set => this.SetValue(IconGlyphProperty, value);
		}

		private static void IconGlyphPropertyChanged (IconGlyphTextBlock self, DependencyPropertyChangedEventArgs e)
		{
			self.Text = ((IconGlyph)e.NewValue).ToUnicode();
		}
	}
}
