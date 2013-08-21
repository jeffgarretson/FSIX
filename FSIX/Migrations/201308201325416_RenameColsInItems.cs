namespace FSIX.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RenameColsInItems : DbMigration
    {
        public override void Up()
        {
            RenameColumn("dbo.Items", "MIMEType", "MimeType");
            RenameColumn("dbo.Items", "ItemType", "Type");
            RenameColumn("dbo.Items", "Name", "FileName");
        }
        
        public override void Down()
        {
            RenameColumn("dbo.Items", "FileName", "Name");
            RenameColumn("dbo.Items", "Type", "ItemType");
            RenameColumn("dbo.Items", "MimeType", "MIMEType");
        }
    }
}
