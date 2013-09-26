using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace FSIX.Models
{
    public partial class Folder
    {
        public Folder()
        {
            this.Items = new HashSet<Item>();
            this.Logs = new HashSet<Log>();
            this.Permissions = new HashSet<Permission>();
        }

        // PK
        public int Id { get; set; }

        // Properties
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime ExpirationDate { get; set; }

        // Navigation
        public virtual ICollection<Item> Items { get; set; }
        public virtual ICollection<Log> Logs { get; set; }
        public virtual ICollection<Permission> Permissions { get; set; }
    }

    #region Configuration
    public class FolderConfiguration : EntityTypeConfiguration<Folder>
    {
        public FolderConfiguration()
        {
            Property(f => f.Name).IsRequired();
            Property(f => f.ExpirationDate).IsRequired().HasColumnType("datetime2");
        }
    }
    #endregion

}
