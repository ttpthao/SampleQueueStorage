namespace SampleQueueStorage.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddMessageTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Messages",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        QueueMessage = c.String(maxLength: 500),
                        CreatedBy = c.String(maxLength: 250),
                        UpdatedBy = c.String(maxLength: 250),
                        CreatedDate = c.DateTime(),
                        UpdatedDate = c.DateTime(),
                        Version = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Messages");
        }
    }
}
