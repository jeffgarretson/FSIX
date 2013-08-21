namespace FSIX.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ItemType : DbMigration
    {
        public override void Up()
        {
            // Allow NULL initially, set ItemType appropriately, and then set NOT NULL
            AddColumn("dbo.Items", "ItemType", c => c.String(nullable: true));
            Sql("UPDATE dbo.Items SET ItemType = CASE WHEN Content IS NULL THEN 'Note' ELSE 'File' END WHERE ItemType IS NULL");
            Sql("ALTER TABLE dbo.Items ALTER COLUMN ItemType nvarchar(max) NOT NULL");
        }
        
        public override void Down()
        {
            DropColumn("dbo.Items", "ItemType");
        }
    }
}
