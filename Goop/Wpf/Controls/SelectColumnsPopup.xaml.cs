namespace Goop.Wpf.Controls
{
	using Goop.ComponentModel;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Windows;
	using System.Windows.Controls;

	/// <summary>
	/// Interaction logic for SelectColumnsPopup.xaml
	/// </summary>
	public partial class SelectColumnsPopup
	{
		private bool updatingAllChecked;

		public SelectColumnsPopup(Window owner)
		{
			this.Owner = owner;
			this.InitializeComponent();
		}

		private static readonly DependencyPropertyKey ColumnsPropertyKey = Gen.Columns(Enumerable.Empty<ColumnItem>());

		public void SetColumns(IEnumerable<GridViewColumn>? visible, IEnumerable<GridViewColumn>? hidden)
		{
			var items = Enumerable.Empty<ColumnItem>();

			if (visible != null)
			{
				items = visible.Select(c => new ColumnItem(c, true));
			}

			if (hidden != null)
			{
				items = items.Concat(hidden.Select(c => new ColumnItem(c, false)));
			}

			this.Columns = (from item in items orderby item.Column.Header select item).ToArray();
			this.UpdateAllChecked();
		}

		public IEnumerable<GridViewColumn> GetVisibleColumns()
		{
			return this.Columns.Where(i => i.IsVisible).Select(i => i.Column);
		}

		public IEnumerable<GridViewColumn> GetHiddenColumns()
		{
			return this.Columns.Where(i => i.IsVisible == false).Select(i => i.Column);
		}

		private void UpdateAllChecked()
		{
			var checks = this.Columns.Select(i => i.IsVisible);
			var check = checks.FirstOrDefault();
			this.allVisibleCheckBox.IsChecked = checks.All(check.Equals) ? check : default(bool?);
		}

		private void Column_OnIsVisibleChanged(object sender, RoutedEventArgs e)
		{
			if (this.updatingAllChecked == false)
			{
				this.UpdateAllChecked();
			}
		}

		private void CheckAll_OnCheckedChanged(object sender, RoutedEventArgs e)
		{
			if (this.allVisibleCheckBox.IsChecked == null)
			{
				return;
			}

			this.updatingAllChecked = true;

			foreach (var item in this.Columns)
			{
				item.IsVisible = this.allVisibleCheckBox.IsChecked.Value;
			}

			this.updatingAllChecked = false;

			// NOTE[Ian] -
			//  A call to 'UpdateAllChecked' here seems appropriate but doesn't work out because
			//  columns that do not allow hiding make us get stuck in the indeterminate state.
			//  This is because a CheckBox goes from indeterminate to unchecked, but we can never
			//  be unchecked (so it gets stuck).
		}

		private void OK_OnClick(object sender, RoutedEventArgs e)
		{
			this.DialogResult = true;
		}

		public class ColumnItem : ObservableObject
		{
			public ColumnItem(GridViewColumn column, bool isVisible)
			{
				this.Column = column;
				this.IsVisible = isVisible;

				var text = new StringBuilder();

				if (string.IsNullOrEmpty(this.Column.HeaderStringFormat) == false)
				{
					text.AppendFormat(this.Column.HeaderStringFormat, this.Column.Header);
				}
				else
				{
					text.Append(this.Column.Header);
				}

				object toolTip = ToolTipService.GetToolTip(this.Column);
				if (toolTip != null)
				{
					text.AppendFormat(" ({0})", toolTip);
				}

				this.Text = text.ToString();
			}

			public GridViewColumn Column { get; }

			private bool _isVisible;
			public bool IsVisible
			{
				get { return this._isVisible; }
				set
				{
					if (value || GridViewHelper.GetAllowHiding(this.Column))
					{
						this.SetField(ref this._isVisible, value);
					}
				}
			}

			public string Text { get; private set; }
		}
	}
}
