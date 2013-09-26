namespace FSIX.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ConvertDateTimeToDateTime2 : DbMigration
    {
        public override void Up()
        {
            DropDefaultConstraint("dbo.Items", "ModifiedTime", q => Sql(q));
            DropDefaultConstraint("dbo.Items", "CreatedTime", q => Sql(q));
            AlterColumn("dbo.Folders", "ExpirationDate", c => c.DateTime(nullable: false, storeType: "datetime2"));
            AlterColumn("dbo.Items", "CreatedTime", c => c.DateTime(nullable: false, storeType: "datetime2"));
            AlterColumn("dbo.Items", "ModifiedTime", c => c.DateTime(nullable: false, storeType: "datetime2"));
            AlterColumn("dbo.Media", "Bytes", c => c.Long(nullable: false));
            AlterColumn("dbo.Media", "CreatedTime", c => c.DateTime(nullable: false, storeType: "datetime2"));
            AlterColumn("dbo.Media", "ModifiedTime", c => c.DateTime(nullable: false, storeType: "datetime2"));
            AlterColumn("dbo.Media", "ExpirationTime", c => c.DateTime(nullable: false, storeType: "datetime2"));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Media", "ExpirationTime", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Media", "ModifiedTime", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Media", "CreatedTime", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Media", "Bytes", c => c.Int(nullable: false));
            AlterColumn("dbo.Items", "ModifiedTime", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Items", "CreatedTime", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Folders", "ExpirationDate", c => c.DateTime(nullable: false));
        }

        public static void DropDefaultConstraint(string tableName, string columnName, Action<string> executeSQL)
        {
            string constraintVariableName = string.Format("@constraint_{0}", Guid.NewGuid().ToString("N"));

            string sql = string.Format(@"
            DECLARE {0} nvarchar(128)
            SELECT {0} = name
            FROM sys.default_constraints
            WHERE parent_object_id = object_id(N'{1}')
            AND col_name(parent_object_id, parent_column_id) = '{2}';
            IF {0} IS NOT NULL
                EXECUTE('ALTER TABLE {1} DROP CONSTRAINT ' + {0})",
                constraintVariableName,
                tableName,
                columnName);

            executeSQL(sql);
        }

    }
}
