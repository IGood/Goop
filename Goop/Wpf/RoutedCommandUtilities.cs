namespace Goop.Wpf
{
	using System;
	using System.Diagnostics.CodeAnalysis;
	using System.Windows.Input;

	public class RoutedCommandUtilities
	{
		public RoutedCommandUtilities(Type ownerType)
		{
			this.OwnerType = ownerType;
		}

		public Type OwnerType { get; }

		public RoutedCommand Create(string name, params InputGesture[] inputGestures)
		{
			return new RoutedCommand(name, OwnerType, new InputGestureCollection(inputGestures));
		}

		public RoutedUICommand CreateUI(string text, string name, params InputGesture[] inputGestures)
		{
			return new RoutedUICommand(text, name, OwnerType, new InputGestureCollection(inputGestures));
		}
	}

	public static class RoutedCommandUtilities<TOwner>
	{
		public delegate void CastedExecutedRoutedEventHandler(TOwner self, ExecutedRoutedEventArgs e);

		public delegate void CastedCanExecuteRoutedEventHandler(TOwner self, CanExecuteRoutedEventArgs e);

		public delegate bool CastedCanExecuteRoutedEventAutoHandler(TOwner self, CanExecuteRoutedEventArgs e);

		public static readonly Type OwnerType = typeof(TOwner);

		private static readonly RoutedCommandUtilities Helper = new RoutedCommandUtilities(OwnerType);

		public static RoutedCommand Create(string name, params InputGesture[] inputGestures)
		{
			return Helper.Create(name, inputGestures);
		}

		public static RoutedUICommand CreateUI(string text, string name, params InputGesture[] inputGestures)
		{
			return Helper.CreateUI(text, name, inputGestures);
		}

		public static void RegisterClassCommandBinding(ICommand command, ExecutedRoutedEventHandler executed, CanExecuteRoutedEventHandler? canExecute = null)
		{
			CommandManager.RegisterClassCommandBinding(OwnerType, new CommandBinding(command, executed, canExecute));
		}

		public static void RegisterClassCommandBinding(ICommand command, CastedExecutedRoutedEventHandler executed, CastedCanExecuteRoutedEventHandler? canExecute = null)
		{
			CommandManager.RegisterClassCommandBinding(OwnerType, new CommandBinding(command, DownCast(executed), DownCast(canExecute)));
		}

		public static void RegisterClassCommandBinding(ICommand command, CastedExecutedRoutedEventHandler executed, CastedCanExecuteRoutedEventAutoHandler? canExecute, bool autoHandle = true)
		{
			if (executed == null)
			{
				throw new ArgumentNullException(nameof(executed));
			}

			ExecutedRoutedEventHandler executedHandler;
			if (autoHandle)
			{
				executedHandler = (s, e) =>
				{
					executed((TOwner)s, e);
					e.Handled = true;
				};
			}
			else
			{
				executedHandler = DownCast(executed);
			}

			CanExecuteRoutedEventHandler? canExecuteHander = null;
			if (canExecute != null)
			{
				if (autoHandle)
				{
					canExecuteHander = (s, e) =>
					{
						e.CanExecute = canExecute((TOwner)s, e);
						e.Handled = true;
					};
				}
				else
				{
					canExecuteHander = (s, e) => e.CanExecute = canExecute((TOwner)s, e);
				}
			}

			CommandManager.RegisterClassCommandBinding(OwnerType, new CommandBinding(command, executedHandler, canExecuteHander));
		}

		private static ExecutedRoutedEventHandler DownCast(CastedExecutedRoutedEventHandler handler)
		{
			if (handler == null)
			{
				throw new ArgumentNullException(nameof(handler));
			}

			return new ExecutedRoutedEventHandler((s, e) => handler((TOwner)s, e));
		}

		[return: NotNullIfNotNull("handler")]
		private static CanExecuteRoutedEventHandler? DownCast(CastedCanExecuteRoutedEventHandler? handler)
		{
			return (handler != null) ? new CanExecuteRoutedEventHandler((s, e) => handler((TOwner)s, e)) : null;
		}
	}
}
