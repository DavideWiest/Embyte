namespace Embyte.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class M11 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.UserNotifications", "Id", "dbo.TestUser");
            DropIndex("dbo.UserNotifications", new[] { "Id" });
            CreateTable(
                "dbo.WebsiteUsage",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Url = c.String(nullable: false, maxLength: 450),
                        RequestCount = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Url, unique: true);
            
            DropTable("dbo.TestUser");
            DropTable("dbo.UserNotifications");
        }
        
        public override void Down()
        {
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
                .PrimaryKey(t => t.NotificationId);
            
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
            
            DropIndex("dbo.WebsiteUsage", new[] { "Url" });
            DropTable("dbo.WebsiteUsage");
            CreateIndex("dbo.UserNotifications", "Id");
            AddForeignKey("dbo.UserNotifications", "Id", "dbo.TestUser", "Id", cascadeDelete: true);
        }
    }
}
