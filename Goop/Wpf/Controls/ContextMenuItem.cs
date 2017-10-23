namespace Goop.Wpf.Controls
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;

    public class ContextMenuItem : MenuItem
    {
        public ContextMenuItem()
        {
            this.SetBinding(
                CommandParameterProperty,
                new Binding
                {
                    RelativeSource = new RelativeSource(RelativeSourceMode.FindAncestor, typeof(ContextMenu), 0),
                    Path = new PropertyPath(nameof(ContextMenu.PlacementTarget)),
                });
        }
    }
}
