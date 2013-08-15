namespace FSIX.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Username = c.String(nullable: false, maxLength: 100),
                        DisplayName = c.String(maxLength: 100),
                        UserType = c.String(maxLength: 5),
                    })
                .PrimaryKey(t => t.Username);
            
            CreateTable(
                "dbo.Permissions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Username = c.String(maxLength: 100),
                        FolderId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.Username)
                .ForeignKey("dbo.Folders", t => t.FolderId)
                .Index(t => t.Username)
                .Index(t => t.FolderId);
            
            CreateTable(
                "dbo.Folders",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Description = c.String(),
                        ExpirationDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Items",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Description = c.String(),
                        MIMEType = c.String(),
                        Content = c.Binary(),
                        FolderId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Folders", t => t.FolderId, cascadeDelete: true)
                .Index(t => t.FolderId);
            
            CreateTable(
                "dbo.Logs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Timestamp = c.DateTime(nullable: false),
                        Message = c.String(nullable: false),
                        CategoryId = c.Int(nullable: false),
                        SeverityId = c.Int(nullable: false),
                        FolderId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Categories", t => t.CategoryId, cascadeDelete: true)
                .ForeignKey("dbo.Severities", t => t.SeverityId, cascadeDelete: true)
                .ForeignKey("dbo.Folders", t => t.FolderId)
                .Index(t => t.CategoryId)
                .Index(t => t.SeverityId)
                .Index(t => t.FolderId);
            
            CreateTable(
                "dbo.Categories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Severities",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Configurations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Value = c.String(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.Logs", new[] { "FolderId" });
            DropIndex("dbo.Logs", new[] { "SeverityId" });
            DropIndex("dbo.Logs", new[] { "CategoryId" });
            DropIndex("dbo.Items", new[] { "FolderId" });
            DropIndex("dbo.Permissions", new[] { "FolderId" });
            DropIndex("dbo.Permissions", new[] { "Username" });
            DropForeignKey("dbo.Logs", "FolderId", "dbo.Folders");
            DropForeignKey("dbo.Logs", "SeverityId", "dbo.Severities");
            DropForeignKey("dbo.Logs", "CategoryId", "dbo.Categories");
            DropForeignKey("dbo.Items", "FolderId", "dbo.Folders");
            DropForeignKey("dbo.Permissions", "FolderId", "dbo.Folders");
            DropForeignKey("dbo.Permissions", "Username", "dbo.Users");
            DropTable("dbo.Configurations");
            DropTable("dbo.Severities");
            DropTable("dbo.Categories");
            DropTable("dbo.Logs");
            DropTable("dbo.Items");
            DropTable("dbo.Folders");
            DropTable("dbo.Permissions");
            DropTable("dbo.Users");
        }
    }
}
