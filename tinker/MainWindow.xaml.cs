using Goop.Wpf.IconGlyph;
using System;
using System.Windows;

namespace tinker
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			this.InitializeComponent();

			this.ic.ItemsSource = Enum.GetValues(typeof(IconGlyph));
		}
	}
}
