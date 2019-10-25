namespace Goop.Wpf
{
    using System.Windows;
    using System.Windows.Media;

    public static class VisualEx
    {
        public static T? GetParent<T>(this DependencyObject reference) where T : Visual
        {
            while (reference != null)
            {
                reference = VisualTreeHelper.GetParent(reference);
                if (reference is T found)
                {
                    return found;
                }
            }

            return null;
        }
    }
}
