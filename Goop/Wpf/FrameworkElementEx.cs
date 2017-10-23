namespace Goop.Wpf
{
    using System;
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
            MethodInfo m = frameworkElementType.GetMethod("GetTemplateChild", BindingFlags.Instance | BindingFlags.NonPublic);
            GetTemplateChildDelegate = (GetTemplateChildFunc)m.CreateDelegate(typeof(GetTemplateChildFunc));

            FieldInfo f = frameworkElementType.GetField("DefaultStyleKeyProperty", BindingFlags.Static | BindingFlags.NonPublic);
            DefaultStyleKeyProperty = (DependencyProperty)f.GetValue(null);
        }

        public static T GetTemplateChild<T>(this FrameworkElement element, string childName) where T : DependencyObject
        {
            return GetTemplateChildDelegate(element, childName) as T;
        }

        public static bool TryGetTemplateChild<T>(this FrameworkElement element, string childName, out T child) where T : DependencyObject
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
                RoutedEventHandler loaded = null;
                loaded = delegate
                {
                    element.Loaded -= loaded;
                    action();
                };
                element.Loaded += loaded;
            }
        }
    }
}
