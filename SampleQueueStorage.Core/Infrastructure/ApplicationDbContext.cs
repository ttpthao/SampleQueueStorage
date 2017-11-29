namespace SampleQueueStorage.Core.Infrastructure
{
    using SampleQueueStorage.Core.Models.Mapping;
    using System.Data.Entity;
    using System.Diagnostics.CodeAnalysis;

    [ExcludeFromCodeCoverage]
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext()
            : base("SampleQueueStorage")
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new MessageMapping());

            base.OnModelCreating(modelBuilder);
        }
    }
}
