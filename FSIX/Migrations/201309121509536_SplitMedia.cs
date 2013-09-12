namespace FSIX.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SplitMedia : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Media",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FileName = c.String(nullable: false),
                        MimeType = c.String(nullable: false),
                        Bytes = c.Int(nullable: false),
                        CreatedTime = c.DateTime(nullable: false),
                        ModifiedTime = c.DateTime(nullable: false),
                        ExpirationTime = c.DateTime(nullable: false),
                        ItemId = c.Int(nullable: false),
                        SubmittedByUsername = c.String(nullable: false, maxLength: 100),
                        LocalFileName = c.String(nullable: false),
                        IV = c.Binary(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Items", t => t.ItemId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.SubmittedByUsername)
                .Index(t => t.ItemId)
                .Index(t => t.SubmittedByUsername);
            
            DropColumn("dbo.Items", "FileName");
            DropColumn("dbo.Items", "MimeType");
            DropColumn("dbo.Items", "Content");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Items", "Content", c => c.Binary());
            AddColumn("dbo.Items", "MimeType", c => c.String());
            AddColumn("dbo.Items", "FileName", c => c.String());
            DropIndex("dbo.Media", new[] { "SubmittedByUsername" });
            DropIndex("dbo.Media", new[] { "ItemId" });
            DropForeignKey("dbo.Media", "SubmittedByUsername", "dbo.Users");
            DropForeignKey("dbo.Media", "ItemId", "dbo.Items");
            DropTable("dbo.Media");
        }
    }
}
