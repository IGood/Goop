namespace Goop.Wpf.IconGlyph
{
	public static class IconGlyphEx
	{
		public static string ToUnicode(this IconGlyph value) => ((char)value).ToString();
	}
}
