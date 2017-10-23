namespace Goop.ObjectModel
{
    using System.Collections;
    using System.Collections.Specialized;
    using System.ComponentModel;
    using System.Windows.Data;

    public static class ObservableCollectionHelpers
    {
        public static readonly PropertyChangedEventArgs CountChangedEventArgs = new PropertyChangedEventArgs(nameof(ICollection.Count));
        public static readonly PropertyChangedEventArgs IndexerChangedEventArgs = new PropertyChangedEventArgs(Binding.IndexerName);
        public static readonly NotifyCollectionChangedEventArgs CollectionResetEventArgs = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset);
    }
}
