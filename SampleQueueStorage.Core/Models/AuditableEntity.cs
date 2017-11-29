namespace SampleQueueStorage.Core.Models
{
    using System;

    public abstract class AuditableEntity<TId> : BaseEntity<TId>
    {
        public virtual string CreatedBy { get; set; }

        public virtual string UpdatedBy { get; set; }

        public virtual DateTime? CreatedDate { get; set; }

        public virtual DateTime? UpdatedDate { get; set; }
    }
}
