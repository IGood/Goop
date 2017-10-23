namespace Goop.ObjectModel
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using Goop.Linq;

    /// <summary>
    /// Represents an observable collection backed by a <see cref="T:System.Collections.Generic.SortedSet{T}" />.
    /// </summary>
    /// <typeparam name="T">The type of elements in the set.</typeparam>
    [Serializable]
    public class ObservableSortedSet<T> : ObservableCollectionBase<T>
    {
        /// <summary>
        /// The sorted set
        /// </summary>
        private readonly SortedSet<T> set;

        /// <summary>
        /// Initializes a new instance of the <see cref="ObservableSortedSet{T}"/> class.
        /// </summary>
        public ObservableSortedSet()
            : this(new SortedSet<T>())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ObservableSortedSet{T}"/> class.
        /// </summary>
        /// <param name="collection">The collection.</param>
        public ObservableSortedSet(IEnumerable<T> collection)
            : this(new SortedSet<T>(collection))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ObservableSortedSet{T}"/> class.
        /// </summary>
        /// <param name="comparer">The comparer.</param>
        public ObservableSortedSet(IComparer<T> comparer)
            : this(new SortedSet<T>(comparer))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ObservableSortedSet{T}"/> class.
        /// </summary>
        /// <param name="collection">The collection.</param>
        /// <param name="comparer">The comparer.</param>
        public ObservableSortedSet(IEnumerable<T> collection, IComparer<T> comparer)
            : this(new SortedSet<T>(collection, comparer))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ObservableSortedSet{T}"/> class.
        /// </summary>
        /// <param name="set">The set.</param>
        private ObservableSortedSet(SortedSet<T> set)
            : base(set)
        {
            this.set = set;
        }

        /// <summary>
        /// Gets the sorted set's comparer.
        /// </summary>
        public IComparer<T> Comparer => this.set.Comparer;

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
