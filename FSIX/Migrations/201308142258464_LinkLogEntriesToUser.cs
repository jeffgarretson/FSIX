namespace FSIX.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class LinkLogEntriesToUser : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Logs", "Username", c => c.String(maxLength: 100));
            AddForeignKey("dbo.Logs", "Username", "dbo.Users", "Username");
            CreateIndex("dbo.Logs", "Username");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Logs", new[] { "Username" });
            DropForeignKey("dbo.Logs", "Username", "dbo.Users");
            DropColumn("dbo.Logs", "Username");
        }
    }
}
