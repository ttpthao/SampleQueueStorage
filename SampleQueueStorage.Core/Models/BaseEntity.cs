namespace SampleQueueStorage.Core.Models
{
    using System;

    public abstract class BaseEntity<TId>
    {
        public virtual TId Id { get; set; }

        public virtual byte[] Version { get; set; }

        public override bool Equals(object obj)
        {
            return this.Equals(obj as BaseEntity<TId>);
        }

        public virtual bool Equals(BaseEntity<TId> other)
        {
            if (other == null)
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            if (IsTransient(this) || IsTransient(other) || !this.Id.Equals(other.Id))
            {
                return false;
            }

            var otherType = other.GetUnproxiedType();
            var thisType = this.GetUnproxiedType();

            return thisType.IsAssignableFrom(otherType) || otherType.IsAssignableFrom(thisType);
        }

        public override int GetHashCode()
        {
            return this.Id.GetHashCode();
        }

        private static bool IsTransient(BaseEntity<TId> obj)
        {
            return obj != null &&
            obj.Id.Equals(default(TId));
        }

        private Type GetUnproxiedType()
        {
            return this.GetType();
        }
    }
}