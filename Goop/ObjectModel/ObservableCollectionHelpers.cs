namespace Goop.ObjectModel
{
    using System;
    using System.Collections;
    using System.Collections.Specialized;
    using System.ComponentModel;
    using System.Windows.Data;

    public static class ObservableCollectionHelpers
    {
        public static readonly PropertyChangedEventArgs CountChangedEventArgs = new PropertyChangedEventArgs(nameof(ICollection.Count));
        public static readonly PropertyChangedEventArgs IndexerChangedEventArgs = new PropertyChangedEventArgs(Binding.IndexerName);
        public static readonly NotifyCollectionChangedEventArgs CollectionResetEventArgs = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset);

        public static void AddHandler(this INotifyCollectionChanged source, EventHandler<NotifyCollectionChangedEventArgs> handler)
        {
            CollectionChangedEventManager.AddHandler(source, handler);
        }

        public static void RemoveHandler(this INotifyCollectionChanged source, EventHandler<NotifyCollectionChangedEventArgs> handler)
        {
            CollectionChangedEventManager.RemoveHandler(source, handler);
        }
    }
}
