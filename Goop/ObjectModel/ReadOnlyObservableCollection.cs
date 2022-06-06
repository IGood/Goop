namespace Goop.ObjectModel
{
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.Collections.Specialized;
	using System.ComponentModel;

	/// <summary>
	/// Represents a read-only observable collection that is backed by an <see cref="ObservableCollectionBase{T}"/>.
	/// </summary>
	/// <typeparam name="T">The type of the elements in the collection.</typeparam>
	public sealed class ReadOnlyObservableCollection<T> : IReadOnlyCollection<T>, ICollection, INotifyCollectionChanged, INotifyPropertyChanged, IDisposable
	{
		private readonly ObservableCollectionBase<T> collection;

		/// <summary>
		/// Initializes a new instance of the <see cref="ReadOnlyObservableCollection{T}"/> class
		/// observing the specified collection.
		/// </summary>
		/// <param name="collection">The collection to be observed.</param>
		public ReadOnlyObservableCollection(ObservableCollectionBase<T> collection)
		{
			this.collection = collection;
			this.collection.CollectionChanged += this.HandleCollectionChanged;
			this.collection.PropertyChanged += this.HandlePropertyChanged;
		}

		/// <summary>
		/// Occurs when the collection changes.
		/// </summary>
		[field: NonSerialized]
		public event NotifyCollectionChangedEventHandler? CollectionChanged;

		/// <summary>
		/// Occurs when a property value changes.
		/// </summary>
		[field: NonSerialized]
		public event PropertyChangedEventHandler? PropertyChanged;

		/// <summary>
		/// Gets the number of elements in the collection.
		/// </summary>
		public int Count => this.collection.Count;

		/// <summary>
		/// Gets a value indicating whether access to the <see cref="T:System.Collections.ICollection" /> is synchronized (thread safe).
		/// </summary>
		bool ICollection.IsSynchronized => this.NonGenCollection.IsSynchronized;

		/// <summary>
		/// Gets an object that can be used to synchronize access to the <see cref="T:System.Collections.ICollection" />.
		/// </summary>
		object ICollection.SyncRoot => this.NonGenCollection.SyncRoot;

		/// <summary>
		/// Gets the internal collection as an <see cref="T:System.Collections.ICollection"/>.
		/// </summary>
		private ICollection NonGenCollection => this.collection;

		/// <summary>
		/// Gets the <see cref="T"/> at the specified index.
		/// </summary>
		/// <param name="index">The index.</param>
		/// <returns>the object at the specified index.</returns>
		public T this[int index] => this.collection[index];

		/// <summary>
		/// Copies the elements of the <see cref="T:System.Collections.ICollection" /> to an <see cref="T:System.Array" />, starting at a particular <see cref="T:System.Array" /> index.
		/// </summary>
		/// <param name="array">The one-dimensional <see cref="T:System.Array" /> that is the destination of the elements copied from <see cref="T:System.Collections.ICollection" />. The <see cref="T:System.Array" /> must have zero-based indexing.</param>
		/// <param name="index">The zero-based index in <paramref name="array" /> at which copying begins.</param>
		void ICollection.CopyTo(Array array, int index)
		{
			this.NonGenCollection.CopyTo(array, index);
		}

		/// <summary>
		/// Returns an enumerator that iterates through the collection.
		/// </summary>
		/// <returns>A <see cref="T:System.Collections.Generic.IEnumerator{T}" /> that can be used to iterate through the collection.</returns>
		public IEnumerator<T> GetEnumerator()
		{
			return this.collection.GetEnumerator();
		}

		/// <summary>
		/// Returns an enumerator that iterates through a collection.
		/// </summary>
		/// <returns>An <see cref="T:System.Collections.IEnumerator" /> object that can be used to iterate through the collection.</returns>
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.collection.GetEnumerator();
		}

		/// <summary>
		/// Unhooks the event handlers from the observed collection.
		/// </summary>
		public void Dispose()
		{
			this.collection.CollectionChanged -= this.HandleCollectionChanged;
			this.collection.PropertyChanged -= this.HandlePropertyChanged;
		}

		private void HandleCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
		{
			this.CollectionChanged?.Invoke(this, e);
		}

		private void HandlePropertyChanged(object? sender, PropertyChangedEventArgs e)
		{
			this.PropertyChanged?.Invoke(this, e);
		}
	}
}
