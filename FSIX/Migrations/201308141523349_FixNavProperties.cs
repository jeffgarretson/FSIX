namespace FSIX.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixNavProperties : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Permissions", "Username", "dbo.Users");
            DropForeignKey("dbo.Permissions", "FolderId", "dbo.Folders");
            DropIndex("dbo.Permissions", new[] { "Username" });
            DropIndex("dbo.Permissions", new[] { "FolderId" });
            AddColumn("dbo.Permissions", "IsOwner", c => c.Boolean(nullable: false));
            AddColumn("dbo.Permissions", "PermRead", c => c.Boolean(nullable: false));
            AddColumn("dbo.Permissions", "PermWrite", c => c.Boolean(nullable: false));
            AddColumn("dbo.Permissions", "PermShare", c => c.Boolean(nullable: false));
            AlterColumn("dbo.Permissions", "Username", c => c.String(nullable: false, maxLength: 100));
            AlterColumn("dbo.Permissions", "FolderId", c => c.Int(nullable: false));
            AddForeignKey("dbo.Permissions", "Username", "dbo.Users", "Username", cascadeDelete: true);
            AddForeignKey("dbo.Permissions", "FolderId", "dbo.Folders", "Id", cascadeDelete: true);
            CreateIndex("dbo.Permissions", "Username");
            CreateIndex("dbo.Permissions", "FolderId");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Permissions", new[] { "FolderId" });
            DropIndex("dbo.Permissions", new[] { "Username" });
            DropForeignKey("dbo.Permissions", "FolderId", "dbo.Folders");
            DropForeignKey("dbo.Permissions", "Username", "dbo.Users");
            AlterColumn("dbo.Permissions", "FolderId", c => c.Int());
            AlterColumn("dbo.Permissions", "Username", c => c.String(maxLength: 100));
            DropColumn("dbo.Permissions", "PermShare");
            DropColumn("dbo.Permissions", "PermWrite");
            DropColumn("dbo.Permissions", "PermRead");
            DropColumn("dbo.Permissions", "IsOwner");
            CreateIndex("dbo.Permissions", "FolderId");
            CreateIndex("dbo.Permissions", "Username");
            AddForeignKey("dbo.Permissions", "FolderId", "dbo.Folders", "Id");
            AddForeignKey("dbo.Permissions", "Username", "dbo.Users", "Username");
        }
    }
}
