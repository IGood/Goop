namespace GoopBenchmark
{
    using BenchmarkDotNet.Attributes;
    using System;
    using System.ComponentModel;

    /// <summary>
    /// Manual implementation?
    /// Delegate implementation to helper; access directly?
    /// Delegate implementation to helper; access via interface?
    /// </summary>
    public class InterfaceImplementations
    {
        private static readonly NoHelp noHelpDirect = new NoHelp();
        private static readonly ISelectable noHelp = new NoHelp();
        private static readonly ISelectable helperDirect = new HelperDirect();
        private static readonly ISelectable helperViaInterface = new HelperViaInterface();

        //*
        static InterfaceImplementations()
        {
            noHelpDirect.PropertyChanged += OnPropertyChanged;
            ((INotifyPropertyChanged)noHelp).PropertyChanged += OnPropertyChanged;
            ((INotifyPropertyChanged)helperViaInterface).PropertyChanged += OnPropertyChanged;
            ((INotifyPropertyChanged)helperDirect).PropertyChanged += OnPropertyChanged;
        }
        //*/

        public static int Changes { get; private set; }

        [Benchmark]
        public bool NoHelpDirect()
        {
            noHelpDirect.IsSelected = true;
            noHelpDirect.IsSelected = false;
            return noHelpDirect.IsSelected;
        }

        [Benchmark]
        public bool NoHelp()
        {
            noHelp.IsSelected = true;
            noHelp.IsSelected = false;
            return noHelp.IsSelected;
        }

        [Benchmark]
        public bool HelperDirect()
        {
            helperDirect.IsSelected = true;
            helperDirect.IsSelected = false;
            return helperDirect.IsSelected;
        }

        [Benchmark]
        public bool HelperViaInterface()
        {
            helperViaInterface.IsSelected = true;
            helperViaInterface.IsSelected = false;
            return helperViaInterface.IsSelected;
        }

        private static void OnPropertyChanged(object sender, EventArgs e)
        {
            ++Changes;
        }
    }

    public interface ISelectable
    {
        bool IsSelected { get; set; }
    }

    public class NoHelp : INotifyPropertyChanged, ISelectable
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public bool IsSelected
        {
            get => this._isSelected;
            set
            {
                if (this._isSelected != value)
                {
                    this._isSelected = value;
                    this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.IsSelected)));
                }
            }
        }
        private bool _isSelected;
    }

    public class SelectableImpl : ISelectable
    {
        public event EventHandler IsSelectedChanged;

        public bool IsSelected
        {
            get => this._isSelected;
            set
            {
                if (this._isSelected != value)
                {
                    this._isSelected = value;
                    this.IsSelectedChanged?.Invoke(this, EventArgs.Empty);
                }
            }
        }
        private bool _isSelected;
    }

    public class HelperDirect : INotifyPropertyChanged, ISelectable
    {
        private readonly SelectableImpl impl = new SelectableImpl();

        public HelperDirect()
        {
            this.impl.IsSelectedChanged += delegate { this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.IsSelected))); };
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public bool IsSelected
        {
            get => this.impl.IsSelected;
            set => this.impl.IsSelected = value;
        }
    }

    public class HelperViaInterface : INotifyPropertyChanged, ISelectable
    {
        private readonly ISelectable impl;

        public HelperViaInterface()
        {
            var impl = new SelectableImpl();
            impl.IsSelectedChanged += delegate { this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.IsSelected))); };
            this.impl = impl;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public bool IsSelected
        {
            get => this.impl.IsSelected;
            set => this.impl.IsSelected = value;
        }
    }
}
