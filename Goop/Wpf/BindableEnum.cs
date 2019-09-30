namespace Goop.Wpf
{
    using System;
    using System.ComponentModel;
    using System.Windows.Data;

    public class BindableEnum<T> : INotifyPropertyChanged where T : struct
    {
        public BindableEnum() { }

        public BindableEnum(T value) => this.Value = value;

        public event PropertyChangedEventHandler PropertyChanged;

        public event EventHandler ValueChanged;

        public T Value
        {
            get => this._value;
            set
            {
                this._value = value;
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.Value)));
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(Binding.IndexerName));
                this.ValueChanged?.Invoke(this, EventArgs.Empty);
            }
        }
        private T _value;

        public bool this[string stringValue]
        {
            get
            {
                return
                    Enum.TryParse(stringValue, true, out T enumValue) &&
                    object.Equals(this.Value, enumValue);
            }
            set
            {
                if (value && Enum.TryParse(stringValue, true, out T enumValue))
                {
                    this.Value = enumValue;
                }
            }
        }

        public static implicit operator T(BindableEnum<T> obj) => obj.Value;
    }

    public static class BindableEnum
    {
        public static BindableEnum<T> Create<T>(T value) where T : struct
        {
            return new BindableEnum<T>(value);
        }
    }
}
