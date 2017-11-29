namespace SampleQueueStorage.Core.Models.Mapping
{
    using System.ComponentModel.DataAnnotations.Schema;

    public abstract class AuditableEntityMapping<T, TId> : BaseEntityMapping<T, TId>
        where T : AuditableEntity<TId>
        where TId : struct
    {
        protected AuditableEntityMapping(DatabaseGeneratedOption databaseGeneratedOption = DatabaseGeneratedOption.Identity)
            : base(databaseGeneratedOption)
        {
            this.Property(x => x.CreatedBy).HasMaxLength(250);
            this.Property(x => x.UpdatedBy).HasMaxLength(250);
        }
    }
}
