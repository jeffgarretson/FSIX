namespace FSIX.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RenameItemsDescToNote : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Items", "Name", c => c.String());
            RenameColumn("dbo.Items", "Description", "Note");
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Items", "Name", c => c.String(nullable: false));
            RenameColumn("dbo.Items", "Note", "Description");
        }
    }
}
