namespace Goop.ObjectModel
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using Goop.Linq;

    /// <summary>
    /// Represents an observable collection backed by a <see cref="T:System.Collections.Generic.HashSet{T}" />.
    /// </summary>
    /// <typeparam name="T">The type of elements in the set.</typeparam>
    [Serializable]
    public class ObservableHashSet<T> : ObservableCollectionBase<T>
    {
        /// <summary>
        /// The sorted set
        /// </summary>
        private readonly HashSet<T> set;

        /// <summary>
        /// Initializes a new instance of the <see cref="ObservableHashSet{T}"/> class.
        /// </summary>
        public ObservableHashSet()
            : this(new HashSet<T>())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ObservableHashSet{T}"/> class.
        /// </summary>
        /// <param name="collection">The collection.</param>
        public ObservableHashSet(IEnumerable<T> collection)
            : this(new HashSet<T>(collection))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ObservableHashSet{T}"/> class.
        /// </summary>
        /// <param name="comparer">The comparer.</param>
        public ObservableHashSet(IEqualityComparer<T> comparer)
            : this(new HashSet<T>(comparer))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ObservableHashSet{T}"/> class.
        /// </summary>
        /// <param name="collection">The collection.</param>
        /// <param name="comparer">The comparer.</param>
        public ObservableHashSet(IEnumerable<T> collection, IEqualityComparer<T> comparer)
            : this(new HashSet<T>(collection, comparer))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ObservableHashSet{T}"/> class.
        /// </summary>
        /// <param name="set">The set.</param>
        private ObservableHashSet(HashSet<T> set)
            : base(set)
        {
            this.set = set;
        }

        /// <summary>
        /// Gets the sorted set's comparer.
        /// </summary>
        public IEqualityComparer<T> Comparer => this.set.Comparer;

        /// <summary>
        /// Adds the specified item.
        /// </summary>
        /// <param name="item">The item to add to the set.</param>
        /// <returns><c>true</c> if item is added to the set; otherwise, <c>false</c>.</returns>
        new public bool Add(T item)
        {
            if (this.set.Add(item))
            {
                int index = this.IndexOf(item);

                this.OnPropertyChanged(ObservableCollectionHelpers.CountChangedEventArgs);
                this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item, index));

                return true;
            }

            return false;
        }

        protected override void AddCore(T item) => this.Add(item);

        /// <summary>
        /// Searches for the specified object and returns the zero-based index of the first
        /// occurrence within the entire set.
        /// </summary>
        /// <param name="value">The object to locate in the set. The value can be null for reference types.</param>
        /// <returns>The zero-based index of the first occurrence of item within the entire
        /// set, if found; otherwise, <c>-1</c>.</returns>
        public override int IndexOf(T value)
        {
            return this.set.IndexOf(value, this.Comparer);
        }
    }
}
