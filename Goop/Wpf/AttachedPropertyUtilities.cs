namespace Goop.Wpf
{
	using System;
	using System.Diagnostics;
	using System.Windows;
	using System.Windows.Data;

	public class AttachedPropertyUtilities
	{
		public AttachedPropertyUtilities(Type ownerType)
		{
			this.OwnerType = ownerType;
			this.ForDO = this.For<DependencyObject>();
		}

		public Type OwnerType { get; }

		public ForType<DependencyObject> ForDO { get; }

		public ForType<TComponent> For<TComponent>() where TComponent : DependencyObject
		{
			return new ForType<TComponent>(this.OwnerType);
		}

		private static string GetPropertyName(Delegate getter, Delegate setter)
		{
			ValidateAccessors(getter, setter);

			return getter.Method.Name.Substring(3);
		}

		[Conditional("DEBUG")]
		private static void ValidateAccessors(Delegate getter, Delegate setter)
		{
			if (getter == null)
			{
				throw new ArgumentNullException(nameof(getter), "Attached Properties must have a getter!");
			}

			if (setter == null)
			{
				throw new ArgumentNullException(nameof(setter), "Attached Properties must have a setter!");
			}

			string getterName = getter.Method.Name;
			string setterName = setter.Method.Name;

			if (!getterName.StartsWith("Get"))
			{
				throw new ArgumentException("Getter Error: Attached properties require a getter formatted Get{name}.", nameof(getter));
			}

			if (!setterName.StartsWith("Set"))
			{
				throw new ArgumentException("Setter Error: Attached properties require a setter formatted Set{name}.", nameof(setter));
			}

			if (getterName.Substring(3) != setterName.Substring(3))
			{
				throw new ArgumentException("Attached properties require a getter and setter formatted Get{name}/Set{name} where the names match.");
			}
		}

		public class ForType<TComponent> where TComponent : DependencyObject
		{
			public delegate T Getter<T>(TComponent component);

			public delegate void Setter<T>(TComponent component, T value);

			private static readonly Type ComponentType = typeof(TComponent);

			public ForType(Type ownerType)
			{
				this.OwnerType = ownerType;
			}

			public Type OwnerType { get; }

			public DependencyProperty Register<T>(Getter<T> getter, Setter<T> setter)
			{
				return DependencyProperty.RegisterAttached(GetPropertyName(getter, setter), typeof(T), this.OwnerType);
			}

			public DependencyProperty Register<T>(Getter<T> getter, Setter<T> setter, PropertyMetadata defaultMetadata)
			{
				return DependencyProperty.RegisterAttached(GetPropertyName(getter, setter), typeof(T), this.OwnerType, defaultMetadata);
			}

			public DependencyProperty Register<T>(Getter<T> getter, Setter<T> setter, PropertyMetadata defaultMetadata, DependencyPropertyUtilities<TComponent>.CastedValidateValueCallback<T>? validateValueCallback)
			{
				return DependencyProperty.RegisterAttached(GetPropertyName(getter, setter), typeof(T), this.OwnerType, defaultMetadata, DependencyPropertyUtilities<TComponent>.DownCast(validateValueCallback));
			}

			public DependencyProperty Register<T>(
				Getter<T> getter,
				Setter<T> setter,
				T defaultValue,
				DependencyPropertyUtilities<TComponent>.CastedPropertyChangedCallback? propertyChangedCallback = null,
				DependencyPropertyUtilities<TComponent>.CastedCoerceValueCallback<T>? coerceValueCallback = null,
				DependencyPropertyUtilities<TComponent>.CastedValidateValueCallback<T>? validateValueCallback = null)
			{
				return Register(
					getter,
					setter,
					DependencyPropertyUtilities<TComponent>.CreatePropertyMetadata(defaultValue, propertyChangedCallback, coerceValueCallback),
					validateValueCallback);
			}

			public DependencyProperty Register<T>(
				Getter<T> getter,
				Setter<T> setter,
				DependencyPropertyUtilities<TComponent>.CastedPropertyChangedCallback propertyChangedCallback,
				DependencyPropertyUtilities<TComponent>.CastedCoerceValueCallback<T>? coerceValueCallback = null,
				DependencyPropertyUtilities<TComponent>.CastedValidateValueCallback<T>? validateValueCallback = null)
			{
				return Register(
					getter,
					setter,
					DependencyPropertyUtilities<TComponent>.CreatePropertyMetadata(propertyChangedCallback, coerceValueCallback),
					validateValueCallback);
			}

			public DependencyProperty Register<T>(
				Getter<T> getter,
				Setter<T> setter,
				T defaultValue,
				FrameworkPropertyMetadataOptions flags,
				DependencyPropertyUtilities<TComponent>.CastedPropertyChangedCallback? propertyChangedCallback = null,
				DependencyPropertyUtilities<TComponent>.CastedCoerceValueCallback<T>? coerceValueCallback = null,
				bool isAnimationProhibited = false,
				UpdateSourceTrigger? defaultUpdateSourceTrigger = null,
				DependencyPropertyUtilities<TComponent>.CastedValidateValueCallback<T>? validateValueCallback = null)
			{
				return Register(
					getter,
					setter,
					DependencyPropertyUtilities<TComponent>.CreateFrameworkPropertyMetadata(
						defaultValue,
						flags,
						propertyChangedCallback,
						coerceValueCallback,
						isAnimationProhibited,
						defaultUpdateSourceTrigger),
					validateValueCallback);
			}

			public DependencyPropertyKey RegisterReadOnly<T>(Getter<T> getter, Setter<T> setter, PropertyMetadata? defaultMetadata = null)
			{
				return DependencyProperty.RegisterAttachedReadOnly(GetPropertyName(getter, setter), typeof(T), this.OwnerType, defaultMetadata);
			}

			public DependencyPropertyKey RegisterReadOnly<T>(Getter<T> getter, Setter<T> setter, PropertyMetadata defaultMetadata, DependencyPropertyUtilities<TComponent>.CastedValidateValueCallback<T>? validateValueCallback)
			{
				return DependencyProperty.RegisterAttachedReadOnly(GetPropertyName(getter, setter), typeof(T), this.OwnerType, defaultMetadata, DependencyPropertyUtilities<TComponent>.DownCast(validateValueCallback));
			}

			public DependencyPropertyKey RegisterReadOnly<T>(
				Getter<T> getter,
				Setter<T> setter,
				T defaultValue,
				DependencyPropertyUtilities<TComponent>.CastedPropertyChangedCallback? propertyChangedCallback = null,
				DependencyPropertyUtilities<TComponent>.CastedCoerceValueCallback<T>? coerceValueCallback = null,
				DependencyPropertyUtilities<TComponent>.CastedValidateValueCallback<T>? validateValueCallback = null)
			{
				return RegisterReadOnly(
					getter,
					setter,
					DependencyPropertyUtilities<TComponent>.CreatePropertyMetadata(defaultValue, propertyChangedCallback, coerceValueCallback),
					validateValueCallback);
			}

			public DependencyPropertyKey RegisterReadOnly<T>(
				Getter<T> getter,
				Setter<T> setter,
				DependencyPropertyUtilities<TComponent>.CastedPropertyChangedCallback propertyChangedCallback,
				DependencyPropertyUtilities<TComponent>.CastedCoerceValueCallback<T>? coerceValueCallback = null,
				DependencyPropertyUtilities<TComponent>.CastedValidateValueCallback<T>? validateValueCallback = null)
			{
				return RegisterReadOnly(
					getter,
					setter,
					DependencyPropertyUtilities<TComponent>.CreatePropertyMetadata(propertyChangedCallback, coerceValueCallback),
					validateValueCallback);
			}

			public DependencyPropertyKey RegisterReadOnly<T>(
				Getter<T> getter,
				Setter<T> setter,
				T defaultValue,
				FrameworkPropertyMetadataOptions flags,
				DependencyPropertyUtilities<TComponent>.CastedPropertyChangedCallback? propertyChangedCallback = null,
				DependencyPropertyUtilities<TComponent>.CastedCoerceValueCallback<T>? coerceValueCallback = null,
				bool isAnimationProhibited = false,
				UpdateSourceTrigger? defaultUpdateSourceTrigger = null,
				DependencyPropertyUtilities<TComponent>.CastedValidateValueCallback<T>? validateValueCallback = null)
			{
				return RegisterReadOnly(
					getter,
					setter,
					DependencyPropertyUtilities<TComponent>.CreateFrameworkPropertyMetadata(
						defaultValue,
						flags,
						propertyChangedCallback,
						coerceValueCallback,
						isAnimationProhibited,
						defaultUpdateSourceTrigger),
					validateValueCallback);
			}
		}
	}
}
