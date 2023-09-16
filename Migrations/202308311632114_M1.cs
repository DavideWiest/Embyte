namespace Embyte.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class M1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TestUser",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        name = c.String(nullable: false),
                        job = c.String(),
                        age = c.Short(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.UserNotifications",
                c => new
                    {
                        NotificationId = c.Int(nullable: false, identity: true),
                        Id = c.Int(nullable: false),
                        Message = c.String(nullable: false),
                        Timestamp = c.DateTime(nullable: false),
                        Status = c.Int(nullable: false),
                        Priority = c.Int(nullable: false),
                        Source = c.String(),
                        ActionLink = c.String(),
                        Metadata = c.String(),
                        ExpirationDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.NotificationId)
                .ForeignKey("dbo.TestUser", t => t.Id, cascadeDelete: true)
                .Index(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserNotifications", "Id", "dbo.TestUser");
            DropIndex("dbo.UserNotifications", new[] { "Id" });
            DropTable("dbo.UserNotifications");
            DropTable("dbo.TestUser");
        }
    }
}
