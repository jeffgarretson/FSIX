namespace FSIX.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ItemMetadata : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Items", "CreatedTime", c => c.DateTime(nullable: false, defaultValueSql: "GETDATE()"));
            AddColumn("dbo.Items", "ModifiedTime", c => c.DateTime(nullable: false, defaultValueSql: "GETDATE()"));

            // AddColumn("dbo.Items", "CreatedByUsername", c => c.String(nullable: false, maxLength: 100));
            // Let's do this in SQL so we can set a default only for the migration
            // and then remove it so future updates must set it explicitly
            Sql("INSERT dbo.Users (Username, DisplayName, UserType) VALUES ('Unknown', 'Unknown User', 'Dummy')");
            Sql("ALTER TABLE dbo.Items ADD CreatedByUsername nvarchar(100) NOT NULL CONSTRAINT DF_Items_CreatedByUsername DEFAULT 'Unknown'");
            Sql("ALTER TABLE dbo.Items DROP CONSTRAINT DF_Items_CreatedByUsername");
            
            AddForeignKey("dbo.Items", "CreatedByUsername", "dbo.Users", "Username", cascadeDelete: true);
            CreateIndex("dbo.Items", "CreatedByUsername");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Items", new[] { "CreatedByUsername" });
            DropForeignKey("dbo.Items", "CreatedByUsername", "dbo.Users");
            DropColumn("dbo.Items", "CreatedByUsername");
            DropColumn("dbo.Items", "ModifiedTime");
            DropColumn("dbo.Items", "CreatedTime");
            Sql("DELETE FROM Users WHERE Username = 'Unknown'");
        }
    }
}
