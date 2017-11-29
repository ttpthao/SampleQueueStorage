using System.ComponentModel.DataAnnotations.Schema;

namespace SampleQueueStorage.Core.Models.Mapping
{
    public class MessageMapping : AuditableEntityMapping<Message, int>
    {
        public MessageMapping(DatabaseGeneratedOption databaseGeneratedOption = DatabaseGeneratedOption.Identity)
            : base(databaseGeneratedOption)
        {
            this.Property(x => x.QueueMessage).HasMaxLength(500);
        }
    }
}
