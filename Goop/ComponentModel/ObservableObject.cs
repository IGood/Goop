namespace Goop.ComponentModel
{
    using System;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;

    [Serializable]
    public abstract class ObservableObject : INotifyPropertyChanged
    {
        [field: NonSerialized]
        public event PropertyChangedEventHandler PropertyChanged;

        protected bool SetField<T>(ref T field, T value, [CallerMemberName] string propertyName = "")
        {
            if (Equals(field, value))
            {
                return false;
            }

            field = value;

            this.OnPropertyChanged(propertyName);

            return true;
        }

        protected bool SetField<T>(T currentValue, T value, Action<T> setter, [CallerMemberName] string propertyName = "")
        {
            if (Equals(currentValue, value))
            {
                return false;
            }

            setter(value);

            this.OnPropertyChanged(propertyName);

            return true;
        }

        /// <summary>
        /// Raises the PropertyChanged event for the specified property name.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            this.PropertyChanged?.Invoke(this, ObservableEx.GetPropertyChangedEventArgs(propertyName));
        }
    }
}
