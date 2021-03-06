﻿using System.Collections.Generic;
using System.Diagnostics;

namespace Goop.ObjectModel
{
	public sealed class CollectionDebugView<T>
	{
		private readonly ICollection<T> collection;

		public CollectionDebugView (ICollection<T> collection) => this.collection = collection;

		[DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
		public T[] Items
		{
			get
			{
				var items = new T[this.collection.Count];
				this.collection.CopyTo(items, 0);
				return items;
			}
		}
	}
}
