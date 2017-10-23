namespace Goop.Wpf
{
    using System;
    using System.Linq.Expressions;
    using System.Windows;
    using System.Windows.Data;

    public static class DependencyPropertyUtilities<TOwner> where TOwner : DependencyObject
    {
        public delegate void CastedPropertyChangedCallback(TOwner self, DependencyPropertyChangedEventArgs e);

        public delegate object CastedCoerceValueCallback<T>(TOwner self, T baseValue);

        public delegate bool CastedValidateValueCallback<T>(T value);

        public static readonly Type OwnerType = typeof(TOwner);

        public static DependencyProperty Register<T>(Expression<Func<TOwner, T>> propertyLamba)
        {
            return DependencyProperty.Register(GetPropertyName(propertyLamba), typeof(T), OwnerType);
        }

        public static DependencyProperty Register<T>(Expression<Func<TOwner, T>> propertyLamba, PropertyMetadata typeMetadata)
        {
            return DependencyProperty.Register(GetPropertyName(propertyLamba), typeof(T), OwnerType, typeMetadata);
        }

        public static DependencyProperty Register<T>(Expression<Func<TOwner, T>> propertyLamba, PropertyMetadata typeMetadata, CastedValidateValueCallback<T> validateValueCallback)
        {
            return DependencyProperty.Register(GetPropertyName(propertyLamba), typeof(T), OwnerType, typeMetadata, DownCast(validateValueCallback));
        }

        public static DependencyProperty Register<T>(
            Expression<Func<TOwner, T>> propertyLamba,
            T defaultValue,
            CastedPropertyChangedCallback propertyChangedCallback = null,
            CastedCoerceValueCallback<T> coerceValueCallback = null,
            CastedValidateValueCallback<T> validateValueCallback = null)
        {
            return Register(
                propertyLamba,
                CreatePropertyMetadata(defaultValue, propertyChangedCallback, coerceValueCallback),
                validateValueCallback);
        }

        public static DependencyProperty Register<T>(
            Expression<Func<TOwner, T>> propertyLamba,
            CastedPropertyChangedCallback propertyChangedCallback,
            CastedCoerceValueCallback<T> coerceValueCallback = null,
            CastedValidateValueCallback<T> validateValueCallback = null)
        {
            return Register(
                propertyLamba,
                CreatePropertyMetadata(propertyChangedCallback, coerceValueCallback),
                validateValueCallback);
        }

        public static DependencyProperty Register<T>(
            Expression<Func<TOwner, T>> propertyLamba,
            T defaultValue,
            FrameworkPropertyMetadataOptions flags,
            CastedPropertyChangedCallback propertyChangedCallback = null,
            CastedCoerceValueCallback<T> coerceValueCallback = null,
            bool isAnimationProhibited = false,
            UpdateSourceTrigger? defaultUpdateSourceTrigger = null,
            CastedValidateValueCallback<T> validateValueCallback = null)
        {
            return Register(
                propertyLamba,
                CreateFrameworkPropertyMetadata(
                    defaultValue,
                    flags,
                    propertyChangedCallback,
                    coerceValueCallback,
                    isAnimationProhibited,
                    defaultUpdateSourceTrigger),
                validateValueCallback);
        }

        public static DependencyPropertyKey RegisterReadOnly<T>(Expression<Func<TOwner, T>> propertyLamba, PropertyMetadata typeMetadata = null)
        {
            return DependencyProperty.RegisterReadOnly(GetPropertyName(propertyLamba), typeof(T), OwnerType, typeMetadata);
        }

        public static DependencyPropertyKey RegisterReadOnly<T>(Expression<Func<TOwner, T>> propertyLamba, PropertyMetadata typeMetadata, CastedValidateValueCallback<T> validateValueCallback)
        {
            return DependencyProperty.RegisterReadOnly(GetPropertyName(propertyLamba), typeof(T), OwnerType, typeMetadata, DownCast(validateValueCallback));
        }

        public static DependencyPropertyKey RegisterReadOnly<T>(
            Expression<Func<TOwner, T>> propertyLamba,
            T defaultValue,
            CastedPropertyChangedCallback propertyChangedCallback = null,
            CastedCoerceValueCallback<T> coerceValueCallback = null,
            CastedValidateValueCallback<T> validateValueCallback = null)
        {
            return RegisterReadOnly(
                propertyLamba,
                CreatePropertyMetadata(defaultValue, propertyChangedCallback, coerceValueCallback),
                validateValueCallback);
        }

        public static DependencyPropertyKey RegisterReadOnly<T>(
            Expression<Func<TOwner, T>> propertyLamba,
            CastedPropertyChangedCallback propertyChangedCallback,
            CastedCoerceValueCallback<T> coerceValueCallback = null,
            CastedValidateValueCallback<T> validateValueCallback = null)
        {
            return RegisterReadOnly(
                propertyLamba,
                CreatePropertyMetadata(propertyChangedCallback, coerceValueCallback),
                validateValueCallback);
        }

        public static DependencyPropertyKey RegisterReadOnly<T>(
            Expression<Func<TOwner, T>> propertyLamba,
            T defaultValue,
            FrameworkPropertyMetadataOptions flags,
            CastedPropertyChangedCallback propertyChangedCallback = null,
            CastedCoerceValueCallback<T> coerceValueCallback = null,
            bool isAnimationProhibited = false,
            UpdateSourceTrigger? defaultUpdateSourceTrigger = null,
            CastedValidateValueCallback<T> validateValueCallback = null)
        {
            return RegisterReadOnly(
                propertyLamba,
                CreateFrameworkPropertyMetadata(
                    defaultValue,
                    flags,
                    propertyChangedCallback,
                    coerceValueCallback,
                    isAnimationProhibited,
                    defaultUpdateSourceTrigger),
                validateValueCallback);
        }

        public static DependencyProperty AddOwner(DependencyProperty dp)
        {
            return dp.AddOwner(OwnerType);
        }

        public static DependencyProperty AddOwner<T>(DependencyProperty dp, T typeMetadata) where T : PropertyMetadata
        {
            return dp.AddOwner(OwnerType, typeMetadata);
        }

        public static DependencyProperty AddOwner<T>(
            DependencyProperty dp,
            T defaultValue,
            CastedPropertyChangedCallback propertyChangedCallback = null,
            CastedCoerceValueCallback<T> coerceValueCallback = null)
        {
            return AddOwner(
                dp,
                CreatePropertyMetadata(defaultValue, propertyChangedCallback, coerceValueCallback));
        }

        public static DependencyProperty AddOwner<T>(
            DependencyProperty dp,
            CastedPropertyChangedCallback propertyChangedCallback,
            CastedCoerceValueCallback<T> coerceValueCallback = null)
        {
            return AddOwner(
                dp,
                CreatePropertyMetadata(propertyChangedCallback, coerceValueCallback));
        }

        public static DependencyProperty AddOwner<T>(
            DependencyProperty dp,
            T defaultValue,
            FrameworkPropertyMetadataOptions flags,
            CastedPropertyChangedCallback propertyChangedCallback = null,
            CastedCoerceValueCallback<T> coerceValueCallback = null,
            bool isAnimationProhibited = false,
            UpdateSourceTrigger? defaultUpdateSourceTrigger = null)
        {
            return AddOwner(
                dp,
                CreateFrameworkPropertyMetadata(
                    defaultValue,
                    flags,
                    propertyChangedCallback,
                    coerceValueCallback,
                    isAnimationProhibited,
                    defaultUpdateSourceTrigger));
        }

        public static void OverrideMetadata<T>(DependencyProperty dp, T typeMetadata) where T : PropertyMetadata
        {
            dp.OverrideMetadata(OwnerType, typeMetadata);
        }

        public static void OverrideMetadata<T>(
            DependencyProperty dp,
            T defaultValue,
            CastedPropertyChangedCallback propertyChangedCallback = null,
            CastedCoerceValueCallback<T> coerceValueCallback = null)
        {
            OverrideMetadata(
                dp,
                CreatePropertyMetadata(defaultValue, propertyChangedCallback, coerceValueCallback));
        }

        public static void OverrideMetadata<T>(
            DependencyProperty dp,
            CastedPropertyChangedCallback propertyChangedCallback,
            CastedCoerceValueCallback<T> coerceValueCallback = null)
        {
            OverrideMetadata(
                dp,
                CreatePropertyMetadata(propertyChangedCallback, coerceValueCallback));
        }

        public static void OverrideMetadata<T>(
            DependencyProperty dp,
            T defaultValue,
            FrameworkPropertyMetadataOptions flags,
            CastedPropertyChangedCallback propertyChangedCallback = null,
            CastedCoerceValueCallback<T> coerceValueCallback = null,
            bool isAnimationProhibited = false,
            UpdateSourceTrigger? defaultUpdateSourceTrigger = null)
        {
            OverrideMetadata(
                dp,
                CreateFrameworkPropertyMetadata(
                    defaultValue,
                    flags,
                    propertyChangedCallback,
                    coerceValueCallback,
                    isAnimationProhibited,
                    defaultUpdateSourceTrigger));
        }

        public static void OverrideDefaultStyleKey()
        {
            FrameworkElementEx.DefaultStyleKeyProperty.OverrideMetadata(OwnerType, new FrameworkPropertyMetadata(OwnerType));
        }

        public static PropertyChangedCallback DownCast(CastedPropertyChangedCallback callback)
        {
            return (callback != null) ? new PropertyChangedCallback((d, e) => callback((TOwner)d, e)) : null;
        }

        public static CoerceValueCallback DownCast<T>(CastedCoerceValueCallback<T> callback)
        {
            return (callback != null) ? new CoerceValueCallback((d, baseValue) => callback((TOwner)d, (T)baseValue)) : null;
        }

        public static ValidateValueCallback DownCast<T>(CastedValidateValueCallback<T> callback)
        {
            return (callback != null) ? new ValidateValueCallback(value => callback((T)value)) : null;
        }

        public static PropertyMetadata CreatePropertyMetadata<T>(
            T defaultValue,
            CastedPropertyChangedCallback propertyChangedCallback,
            CastedCoerceValueCallback<T> coerceValueCallback)
        {
            return new PropertyMetadata(defaultValue, DownCast(propertyChangedCallback), DownCast(coerceValueCallback));
        }

        public static PropertyMetadata CreatePropertyMetadata<T>(
            CastedPropertyChangedCallback propertyChangedCallback,
            CastedCoerceValueCallback<T> coerceValueCallback)
        {
            return new PropertyMetadata(DownCast(propertyChangedCallback)) { CoerceValueCallback = DownCast(coerceValueCallback) };
        }

        public static PropertyMetadata CreateUIPropertyMetadata<T>(
            T defaultValue,
            CastedPropertyChangedCallback propertyChangedCallback,
            CastedCoerceValueCallback<T> coerceValueCallback,
            bool isAnimationProhibited)
        {
            return new UIPropertyMetadata(defaultValue, DownCast(propertyChangedCallback), DownCast(coerceValueCallback), isAnimationProhibited);
        }

        public static PropertyMetadata CreateFrameworkPropertyMetadata<T>(
            T defaultValue,
            FrameworkPropertyMetadataOptions flags,
            CastedPropertyChangedCallback propertyChangedCallback = null,
            CastedCoerceValueCallback<T> coerceValueCallback = null,
            bool isAnimationProhibited = false,
            UpdateSourceTrigger? defaultUpdateSourceTrigger = null)
        {
            return defaultUpdateSourceTrigger.HasValue
                ? new FrameworkPropertyMetadata(defaultValue, flags, DownCast(propertyChangedCallback), DownCast(coerceValueCallback), isAnimationProhibited, defaultUpdateSourceTrigger.Value)
                : new FrameworkPropertyMetadata(defaultValue, flags, DownCast(propertyChangedCallback), DownCast(coerceValueCallback), isAnimationProhibited);
        }

        private static string GetPropertyName<T>(Expression<Func<TOwner, T>> propertyLamba)
        {
            if (propertyLamba == null)
            {
                throw new ArgumentNullException(nameof(propertyLamba));
            }

            if (propertyLamba.Body is MemberExpression expression)
            {
                return expression.Member.Name;
            }

            throw new ArgumentException("Lamba must be a member expression.", nameof(propertyLamba));
        }
    }
}
