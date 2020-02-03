using Goop.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;

namespace Goop.ObjectModel
{
	/// <summary>
	/// Represents a base class for observable collections.
	/// </summary>
	/// <typeparam name="T">The type of the elements in the collection.</typeparam>
	[Serializable]
	[DebuggerTypeProxy(typeof(CollectionDebugView<>))]
	[DebuggerDisplay("Count = {Count}")]
	public abstract class ObservableCollectionBase<T> : ICollection<T>, ICollection, IReadOnlyCollection<T>, INotifyCollectionChanged, INotifyPropertyChanged
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="ObservableCollectionBase{T}"/> class
		/// that uses the specified collection as its storage.
		/// </summary>
		/// <param name="collection">The collection instance.</param>
		protected ObservableCollectionBase (ICollection<T> collection) => this.Collection = collection;

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
		public int Count => this.Collection.Count;

		/// <summary>
		/// Gets a value indicating whether the collection is read-only.
		/// </summary>
		bool ICollection<T>.IsReadOnly => this.Collection.IsReadOnly;

		/// <summary>
		/// Gets a value indicating whether access to the collection is synchronized (thread safe).
		/// </summary>
		bool ICollection.IsSynchronized => this.NonGenCollection?.IsSynchronized ?? false;

		/// <summary>
		/// Gets an object that can be used to synchronize access to the collection.
		/// </summary>
		object ICollection.SyncRoot => this.NonGenCollection?.SyncRoot ?? this;

		/// <summary>
		/// Gets the internal collection.
		/// </summary>
		protected ICollection<T> Collection { get; }

		/// <summary>
		/// Gets the internal collection as an <see cref="T:System.Collections.ICollection"/>.
		/// </summary>
		private ICollection? NonGenCollection => this.Collection as ICollection;
		/// <summary>
		/// Gets the <see cref="T" /> at the specified index.
		/// </summary>
		/// <param name="index">The index.</param>
		/// <returns>the object at the specified index.</returns>
		/// <exception cref="System.NotSupportedException">This collection type does not support indexing.</exception>
		public virtual T this[int index]
		{
			get => throw new NotSupportedException($"The {this.GetType().Name} type does not support indexing.");
			set => throw new NotSupportedException($"The {this.GetType().Name} type does not support indexing.");
		}

		/// <summary>
		/// Adds an item to the collection.
		/// </summary>
		/// <param name="item">The object to add to the collection.</param>
		public void Add (T item) => this.AddCore(item);

		protected virtual void AddCore (T item)
		{
			int index = this.Count;
			this.Collection.Add(item);
			this.OnPropertyChanged(ObservableCollectionHelpers.CountChangedEventArgs);
			this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item, index));
		}

		/// <summary>
		/// Removes the first occurrence of a specific object from the collection.
		/// </summary>
		/// <param name="item">The object to remove from the collection.</param>
		/// <returns><c>true</c> if <paramref name="item" /> was successfully removed from the collection;
		/// otherwise, <c>false</c>. This method also returns <c>false</c> if <paramref name="item" /> is
		/// not found in the collection.</returns>
		public virtual bool Remove (T item)
		{
			int index = this.IndexOf(item);
			if (index != -1 && this.Collection.Remove(item))
			{
				this.OnPropertyChanged(ObservableCollectionHelpers.CountChangedEventArgs);
				this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, item, index));

				return true;
			}

			return false;
		}

		/// <summary>
		/// Removes all items from the collection.
		/// </summary>
		public virtual void Clear ()
		{
			if (this.Count != 0)
			{
				this.Collection.Clear();
				this.OnPropertyChanged(ObservableCollectionHelpers.CountChangedEventArgs);
				this.OnCollectionChanged(ObservableCollectionHelpers.CollectionResetEventArgs);
			}
		}

		/// <summary>
		/// Clears the collection and sets its contents to the elements of the specified collection.
		/// </summary>
		/// <param name="collection">The elements that will replace the contents of the collection.</param>
		public virtual void Reset (IEnumerable<T> collection)
		{
			this.Collection.Clear();
			collection.ForEach(this.Collection.Add);
			this.OnPropertyChanged(ObservableCollectionHelpers.CountChangedEventArgs);
			this.OnCollectionChanged(ObservableCollectionHelpers.CollectionResetEventArgs);
		}

		/// <summary>
		/// Determines whether the collection contains a specific value.
		/// </summary>
		/// <param name="item">The object to locate in the collection.</param>
		/// <returns><c>true</c> if <paramref name="item" /> is found in the collection; otherwise,
		/// <c>false</c>.</returns>
		public bool Contains (T item)
		{
			return this.Collection.Contains(item);
		}

		public virtual int IndexOf (T item)
		{
			return this.Collection.IndexOf(item);
		}

		/// <summary>
		/// Copies the elements of the collection to an array, starting at a particular index.
		/// </summary>
		/// <param name="array">The destination array.</param>
		/// <param name="index">The zero-based index in <paramref name="array" /> at which copying
		/// begins.</param>
		public void CopyTo (T[] array, int index)
		{
			this.Collection.CopyTo(array, index);
		}

		/// <summary>
		/// Returns an enumerator that iterates through the collection.
		/// </summary>
		/// <returns>A <see cref="T:System.Collections.Generic.IEnumerator{T}" /> that can be used to
		/// iterate through the collection.</returns>
		public IEnumerator<T> GetEnumerator ()
		{
			return this.Collection.GetEnumerator();
		}

		/// <summary>
		/// Copies the elements of the collection to an array, starting at a particular index.
		/// </summary>
		/// <param name="array">The one-dimensional <see cref="T:System.Array" /> that is the destination
		/// of the elements copied from the collection. The <see cref="T:System.Array" /> must have
		/// zero-based indexing.</param>
		/// <param name="index">The zero-based index in <paramref name="array" /> at which copying begins.</param>
		void ICollection.CopyTo (Array array, int index)
		{
			foreach (var item in this.Collection)
			{
				array.SetValue(item, index++);
			}
		}

		/// <summary>
		/// Returns an enumerator that iterates through a collection.
		/// </summary>
		/// <returns>An <see cref="T:System.Collections.IEnumerator" /> object that can be used to
		/// iterate through the collection.</returns>
		IEnumerator IEnumerable.GetEnumerator ()
		{
			return this.Collection.GetEnumerator();
		}

		/// <summary>
		/// Raises the <see cref="E:CollectionChanged" /> event.
		/// </summary>
		/// <param name="args">The <see cref="NotifyCollectionChangedEventArgs"/> instance containing the event data.</param>
		protected virtual void OnCollectionChanged (NotifyCollectionChangedEventArgs args)
		{
			this.CollectionChanged?.Invoke(this, args);
		}

		/// <summary>
		/// Raises the <see cref="E:PropertyChanged" /> event.
		/// </summary>
		/// <param name="args">The <see cref="PropertyChangedEventArgs"/> instance containing the event data.</param>
		protected virtual void OnPropertyChanged (PropertyChangedEventArgs args)
		{
			this.PropertyChanged?.Invoke(this, args);
		}
	}
}
