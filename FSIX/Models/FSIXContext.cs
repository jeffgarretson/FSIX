using System.Data.Entity;
using System.Data.Entity.Infrastructure;
//using FSIX.Models.Mapping;

namespace FSIX.Models
{
    public partial class FSIXContext : DbContext
    {
        static FSIXContext()
        {
            Database.SetInitializer<FSIXContext>(null);
        }

        public FSIXContext()
            : base("Name=FSIXContext")
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<Folder> Folders { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<Media> Media { get; set; }
        public DbSet<MediaInternalData> MediaInternalData { get; set; }

        public DbSet<Log> Logs { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Severity> Severities { get; set; }

        public DbSet<Configuration> Configurations { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new UserConfiguration());
            modelBuilder.Configurations.Add(new PermissionConfiguration());
            modelBuilder.Configurations.Add(new FolderConfiguration());
            modelBuilder.Configurations.Add(new ItemConfiguration());
            modelBuilder.Configurations.Add(new MediaConfiguration());
            modelBuilder.Configurations.Add(new MediaInternalDataConfiguration());
        }
    }
}
