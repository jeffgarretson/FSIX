namespace FSIX.Migrations
{
    using FSIX.Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<FSIX.Models.FSIXContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(FSIX.Models.FSIXContext context)
        {

            context.Users.AddOrUpdate(
                p => p.Username,
                new User { Username = "Admin", DisplayName = "Admin", UserType = "Admin" }
            );

            context.Severities.AddOrUpdate(
                p => p.Name,
                new Severity { Name = "Debug", Description = "Detailed information useful for debugging" },
                new Severity { Name = "Info", Description = "Informational messages or success audits" },
                new Severity { Name = "Warn", Description = "Warnings or failure audits" },
                new Severity { Name = "Error", Description = "Errors" },
                new Severity { Name = "Alert", Description = "Serious errors that require immediate attention" }
            );

            context.Categories.AddOrUpdate(
                p => p.Name,
                new Category { Name = "Security", Description = "Security events (new user, login, logout, change pw, etc.)" },
                new Category { Name = "System", Description = "System activity" },
                new Category { Name = "User Actions", Description = "User actions (upload, download, create folder, change folder permissions, etc.)" }
            );

        }
    }
}
