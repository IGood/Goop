namespace Goop.Wpf.Controls
{
	using System;
	using System.Collections;
	using System.Windows;
	using System.Windows.Controls;
	using DP = DependencyPropertyUtilities<EnumComboBox>;

	public partial class EnumComboBox : ComboBox
	{
		static EnumComboBox ()
		{
			DP.OverrideMetadata(SelectedValueProperty, new FrameworkPropertyMetadata(DP.DownCast(OnSelectedValueChanged)));
			DP.OverrideMetadata(ItemsSourceProperty, new FrameworkPropertyMetadata(null, DP.DownCast<IEnumerable?>(CoerceItemsSource)));
		}

		public static readonly DependencyProperty EnumTypeProperty = Gen.EnumType<Type?>();

		private static void OnEnumTypeChanged (EnumComboBox self, DependencyPropertyChangedEventArgs e)
		{
			self.CoerceValue(ItemsSourceProperty);
		}

		private static Type? CoerceEnumType (EnumComboBox self, Type? baseValue)
		{
			return baseValue ?? self.SelectedValue?.GetType();
		}

		private static void OnSelectedValueChanged (EnumComboBox self, DependencyPropertyChangedEventArgs e)
		{
			self.CoerceValue(EnumTypeProperty);
		}

		private static IEnumerable? CoerceItemsSource (EnumComboBox self, IEnumerable? baseValue)
		{
			if (baseValue == null)
			{
				Type? type = self.EnumType;
				if (type?.IsEnum == true)
				{
					return type.GetEnumValues();
				}
			}

			return baseValue;
		}
	}
}
