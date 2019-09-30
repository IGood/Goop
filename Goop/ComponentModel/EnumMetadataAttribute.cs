namespace Goop.ComponentModel
{
    using System;

    [AttributeUsage(AttributeTargets.Field, AllowMultiple = true, Inherited = false)]
    public sealed class EnumMetadataAttribute : Attribute
    {
        public static readonly EnumMetadataAttribute Default = new EnumMetadataAttribute(null);

        public EnumMetadataAttribute(object metadata)
        {
            this.Metadata = metadata;
        }

        public EnumMetadataAttribute(object key, object metadata)
        {
            this.Key = key;
            this.Metadata = metadata;
        }

        public object Key { get; }

        public object Metadata { get; }

        public override object TypeId => this;

        public override bool Match(object obj) => this.Equals(obj);

        public override bool IsDefaultAttribute() => this.Key == Default.Key && this.Metadata == Default.Metadata;

        public override bool Equals(object value)
        {
            if (object.ReferenceEquals(value, null))
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
