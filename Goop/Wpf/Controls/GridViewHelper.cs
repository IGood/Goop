namespace Goop.Wpf.Controls
{
	using Goop.Wpf;
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.Diagnostics.CodeAnalysis;
	using System.Linq;
	using System.Windows;
	using System.Windows.Controls;
	using System.Windows.Data;
	using System.Windows.Input;
	using Cmd = RoutedCommandUtilities<GridViewHelper>;
	using SorterCollection = System.Collections.Generic.List<System.Tuple<System.Collections.IComparer, int>>;

	public sealed partial class GridViewHelper
	{
		private static readonly AttachedPropertyUtilities AP = new AttachedPropertyUtilities(typeof(GridViewHelper));

		public static readonly RoutedUICommand HideColumn = Cmd.CreateUI("Hi_de Column", nameof(HideColumn));
		public static readonly RoutedUICommand SelectColumns = Cmd.CreateUI("_Select Columns", nameof(SelectColumns));

		public static readonly DependencyProperty AllowHidingProperty = GenAttached<GridViewColumn>.AllowHiding<bool>();

		private static readonly DependencyPropertyKey HiddenColumnsPropertyKey = GenAttached<GridView>.HiddenColumns<List<GridViewColumn>?>();

		private static bool CheckRequirements([NotNullWhen(true)] ListView? listView, [NotNullWhen(true)] GridViewColumnHeader? header, [NotNullWhen(true)] out GridView? gridView)
		{
			gridView = listView?.View as GridView;
			return gridView != null && header?.Column != null;
		}

		public static void HideColumn_OnExecuted(object sender, ExecutedRoutedEventArgs e)
		{
			var header = e.OriginalSource as GridViewColumnHeader;
			GridView? gridView;
			if (!CheckRequirements(sender as ListView, header, out gridView) ||
				GetAllowHiding(header.Column) == false)
			{
				return;
			}

			gridView.Columns.Remove(header.Column);

			var hiddenColumns = GetHiddenColumns(gridView);
			if (hiddenColumns == null)
			{
				hiddenColumns = new List<GridViewColumn>(gridView.Columns.Count);
				SetHiddenColumns(gridView, hiddenColumns);
			}

			hiddenColumns.Add(header.Column);

			RemoveColumnComparer(GetColumnSorters(gridView), header.Column);
		}

		public static void HideColumn_OnCanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			var header = e.OriginalSource as GridViewColumnHeader;
			if (CheckRequirements(sender as ListView, header, out _))
			{
				e.CanExecute = GetAllowHiding(header.Column);
				e.Handled = true;
			}
		}

		private static bool CheckRequirements([NotNullWhen(true)] ListView? listView, [NotNullWhen(true)] out GridView? gridView)
		{
			gridView = listView?.View as GridView;
			return gridView != null;
		}

		public static void SelectColumns_OnExecuted(object sender, ExecutedRoutedEventArgs e)
		{
			GridView? gridView;
			if (!CheckRequirements(sender as ListView, out gridView))
			{
				return;
			}

			var pop = new SelectColumnsPopup(Window.GetWindow(gridView));
			pop.SetColumns(gridView.Columns, GetHiddenColumns(gridView));
			if (pop.ShowDialog() == true)
			{
				var hiddenColumns = pop.GetHiddenColumns().ToList();
				SetHiddenColumns(gridView, hiddenColumns);

				var sorters = GetColumnSorters(gridView);
				foreach (var column in hiddenColumns)
				{
					RemoveColumnComparer(sorters, column);
				}

				// Use "concat" then "distinct" to maintain order of visible columns.
				var columns = gridView.Columns
					.Concat(pop.GetVisibleColumns())
					.Distinct()
					.Except(hiddenColumns)
					.ToList();

				gridView.Columns.Clear();
				columns.ForEach(gridView.Columns.Add);
			}

			e.Handled = true;
		}

		private static void RemoveColumnComparer(SorterCollection? sorters, GridViewColumn column)
		{
			var comparer = GetSortComparer(column);
			if (comparer != null)
			{
				var sorter = sorters?.FirstOrDefault(t => t.Item1 == comparer);
				if (sorter != null)
				{
					sorters!.Remove(sorter);
				}
			}
		}

		public static readonly DependencyProperty SortComparerProperty = GenAttached<GridViewColumn>.SortComparer<IComparer?>();

		private static readonly DependencyPropertyKey ColumnSortersPropertyKey = GenAttached<GridView>.ColumnSorters<SorterCollection?>();

		public static void ColumnHeader_OnMouseLeftButtonDown(GridViewColumnHeader header)
		{
			if (header?.Column == null)
			{
				return;
			}

			var comparer = GetSortComparer(header.Column);
			var listView = header.GetParent<ListView>();
			var gridView = listView?.View as GridView;
			var listCollectionView = CollectionViewSource.GetDefaultView(listView?.ItemsSource) as ListCollectionView;
			if (comparer == null || gridView == null || listCollectionView?.CanSort != true)
			{
				return;
			}

			var sorters = GetColumnSorters(gridView);
			if (sorters == null)
			{
				sorters = new SorterCollection();
				SetColumnSorters(gridView, sorters);
			}

			var sorter = sorters.FirstOrDefault(t => t.Item1 == comparer);
			if (sorter != null)
			{
				sorters.Remove(sorter);
				sorter = Tuple.Create(sorter.Item1, -sorter.Item2);
			}
			else
			{
				sorter = Tuple.Create(comparer, 1);
			}

			sorters.Insert(0, sorter);

			listCollectionView.CustomSort = new MultiSorter(sorters);
		}

		private class MultiSorter : IComparer
		{
			private readonly SorterCollection sorters;

			public MultiSorter(SorterCollection sorters) => this.sorters = sorters;

			public int Compare(object? x, object? y)
			{
				int result = 0;
				foreach (var sorter in this.sorters)
				{
					result = sorter.Item1.Compare(x, y) * sorter.Item2;
					if (result != 0)
					{
						break;
					}
				}

				return result;
			}
		}
	}
}
