namespace SampleQueueStorage.Core.Migrations
{
    using System.Data.Entity.Migrations;
    using SampleQueueStorage.Core.Infrastructure;

    internal sealed class Configuration : DbMigrationsConfiguration<ApplicationDbContext>
    {
        public Configuration()
        {
            this.AutomaticMigrationsEnabled = false;
        }
    }
}
