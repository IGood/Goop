namespace Goop.Wpf.Controls
{
    using System.Windows.Controls;
    using System.Windows.Data;

    public class ContextMenuItem : MenuItem
    {
        public ContextMenuItem()
        {
            this.SetBinding(
                MenuItem.CommandTargetProperty,
                new Binding(nameof(ContextMenu.PlacementTarget))
                {
                    RelativeSource = new RelativeSource(RelativeSourceMode.FindAncestor, typeof(ContextMenu), 0)
                });
        }
    }
}
