namespace Goop.ComponentModel
{
    using System;
    using System.Collections.Concurrent;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;

    [Serializable]
    public abstract class ObservableObject : INotifyPropertyChanged
    {
        private static readonly ConcurrentDictionary<string, PropertyChangedEventArgs> EventArgsCache = new ConcurrentDictionary<string, PropertyChangedEventArgs>();

        [field: NonSerialized]
        public event PropertyChangedEventHandler PropertyChanged;

        protected bool SetField<T>(ref T field, T value, [CallerMemberName] string propertyName = "")
        {
            if (Equals(field, value))
            {
                return false;
            }

            field = value;

            this.OnPropertyChanged(propertyName);

            return true;
        }

        /// <summary>
        /// Returns an instance of PropertyChangedEventArgs for the specified property name.
        /// </summary>
        /// <param name="propertyName">Name of the property to get event args for.</param>
        /// <returns>an instance of PropertyChangedEventArgs for the specified property name.</returns>
        private static PropertyChangedEventArgs GetPropertyChangedEventArgs(string propertyName)
        {
            return EventArgsCache.GetOrAdd(propertyName, name => new PropertyChangedEventArgs(name));
        }

        /// <summary>
        /// Raises the PropertyChanged event for the specified property name.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            this.PropertyChanged?.Invoke(this, GetPropertyChangedEventArgs(propertyName));
        }

        public void AddHandler(EventHandler<PropertyChangedEventArgs> handler, string propertyName)
        {
            PropertyChangedEventManager.AddHandler(this, handler, propertyName);
        }

        public void RemoveHandler(EventHandler<PropertyChangedEventArgs> handler, string propertyName)
        {
            PropertyChangedEventManager.RemoveHandler(this, handler, propertyName);
        }
    }
}
