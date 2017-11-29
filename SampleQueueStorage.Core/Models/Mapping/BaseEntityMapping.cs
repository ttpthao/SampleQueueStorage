namespace SampleQueueStorage.Core.Models.Mapping
{
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.ModelConfiguration;

    public abstract class BaseEntityMapping<T, TId> : EntityTypeConfiguration<T>
        where T : BaseEntity<TId>
        where TId : struct
    {
        protected BaseEntityMapping(DatabaseGeneratedOption databaseGeneratedOption = DatabaseGeneratedOption.Identity)
        {
            this.HasKey(x => x.Id)
               .Property(t => t.Id)
               .HasDatabaseGeneratedOption(databaseGeneratedOption);
            this.Property(x => x.Version).IsRowVersion();
        }
    }
}
