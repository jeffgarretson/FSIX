namespace FSIX.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ItemTypeMaxLength : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Items", "ItemType", c => c.String(nullable: false, maxLength: 10));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Items", "ItemType", c => c.String(nullable: false));
        }
    }
}
