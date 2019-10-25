namespace Goop.Wpf
{
	using System;
	using System.Diagnostics.CodeAnalysis;
	using System.Reflection;
	using System.Windows;
	using GetTemplateChildFunc = System.Func<System.Windows.FrameworkElement, string, System.Windows.DependencyObject>;

	public static class FrameworkElementEx
	{
		private static readonly GetTemplateChildFunc GetTemplateChildDelegate;

		internal static readonly DependencyProperty DefaultStyleKeyProperty;

		static FrameworkElementEx()
		{
			Type frameworkElementType = typeof(FrameworkElement);
			MethodInfo m = frameworkElementType.GetMethod("GetTemplateChild", BindingFlags.Instance | BindingFlags.NonPublic)!;
			GetTemplateChildDelegate = (GetTemplateChildFunc)m.CreateDelegate(typeof(GetTemplateChildFunc));

			FieldInfo f = frameworkElementType.GetField("DefaultStyleKeyProperty", BindingFlags.Static | BindingFlags.NonPublic)!;
			DefaultStyleKeyProperty = (DependencyProperty)f.GetValue(null)!;
		}

		public static T? GetTemplateChild<T>(this FrameworkElement element, string childName) where T : DependencyObject
		{
			return GetTemplateChildDelegate(element, childName) as T;
		}

		public static bool TryGetTemplateChild<T>(this FrameworkElement element, string childName, [NotNullWhen(true)] out T? child) where T : DependencyObject
		{
			child = element.GetTemplateChild<T>(childName);
			return child != null;
		}

		public static void WhenFirstLoaded(this FrameworkElement element, Action action)
		{
			if (element.IsLoaded)
			{
				action();
			}
			else
			{
				element.Loaded += _OnLoaded;

				void _OnLoaded(object sender, RoutedEventArgs e)
				{
					element.Loaded -= _OnLoaded;
					action();
				}
			}
		}
	}
}
