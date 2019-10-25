namespace Goop.ComponentModel
{
	using System;

	[AttributeUsage(AttributeTargets.Field, AllowMultiple = true)]
	public sealed class EnumMetadataAttribute : Attribute
	{
		public EnumMetadataAttribute(object metadata)
		{
			this.Metadata = metadata;
		}

		public EnumMetadataAttribute(object key, object metadata)
		{
			this.Key = key;
			this.Metadata = metadata;
		}

		public object? Key { get; }

		public object Metadata { get; }

		public override object TypeId => this;

		public override bool Equals(object? value)
		{
			if (value is null)
			{
				return false;
			}

			if (object.ReferenceEquals(value, this))
			{
				return true;
			}

			return
				value is EnumMetadataAttribute other &&
				this.Key == other.Key &&
				this.Metadata == other.Metadata;
		}

		public override int GetHashCode() => (this.Key?.GetHashCode() ?? 0) ^ (this.Metadata?.GetHashCode() ?? 0);
	}
}
