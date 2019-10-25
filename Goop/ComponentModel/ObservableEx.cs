namespace Goop.ComponentModel
{
	using System;
	using System.Collections.Concurrent;
	using System.ComponentModel;
	using System.Runtime.CompilerServices;

	public static class ObservableEx
	{
		private static readonly ConcurrentDictionary<string, PropertyChangedEventArgs> EventArgsCache = new ConcurrentDictionary<string, PropertyChangedEventArgs>();

		/// <summary>
		/// Returns an instance of PropertyChangedEventArgs for the specified property name.
		/// </summary>
		/// <param name="propertyName">Name of the property to get event args for.</param>
		/// <returns>an instance of PropertyChangedEventArgs for the specified property name.</returns>
		public static PropertyChangedEventArgs GetPropertyChangedEventArgs(string propertyName)
		{
			return EventArgsCache.GetOrAdd(propertyName, name => new PropertyChangedEventArgs(name));
		}

		public static bool SetField<T>(this object source, ref T field, T value, EventHandler? notify, [CallerMemberName] string propertyName = "")
		{
			if (Equals(field, value))
			{
				return false;
			}

			field = value;

			notify?.Invoke(source, GetPropertyChangedEventArgs(propertyName));

			return true;
		}

		/// <summary>
		/// Raises the PropertyChanged event for the specified property name.
		/// </summary>
		/// <param name="propertyName">Name of the property.</param>
		public static void OnPropertyChanged(this object source, EventHandler? notify, [CallerMemberName] string propertyName = "")
		{
			notify?.Invoke(source, GetPropertyChangedEventArgs(propertyName));
		}

		public static void AddHandler(INotifyPropertyChanged source, EventHandler<PropertyChangedEventArgs> handler, string propertyName = "")
		{
			PropertyChangedEventManager.AddHandler(source, handler, propertyName);
		}

		public static void RemoveHandler(INotifyPropertyChanged source, EventHandler<PropertyChangedEventArgs> handler, string propertyName = "")
		{
			PropertyChangedEventManager.RemoveHandler(source, handler, propertyName);
		}
	}
}
