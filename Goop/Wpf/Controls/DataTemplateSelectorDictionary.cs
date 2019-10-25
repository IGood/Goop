namespace Goop.Wpf.Controls
{
	using System.Collections.Generic;
	using System.Windows;
	using System.Windows.Controls;
	using System.Windows.Markup;

	[ContentProperty("Templates")]
	public class DataTemplateSelectorDictionary : DataTemplateSelector
	{
		public Dictionary<object, DataTemplate> Templates { get; set; } = new Dictionary<object, DataTemplate>();

		public override DataTemplate? SelectTemplate(object? item, DependencyObject container)
		{
			if (item != null && this.Templates.TryGetValue(item, out DataTemplate? result))
			{
				return result;
			}

			return base.SelectTemplate(item, container);
		}
	}
}
