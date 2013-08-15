using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace FSIX.Models
{
    public partial class Permission
    {
        // Primary Key
        public int Id { get; set; }

        // Foreign Keys
        public string Username { get; set; }
        public int FolderId { get; set; }

        // Properties
        public Boolean IsOwner { get; set; }
        public Boolean PermRead { get; set; }
        public Boolean PermWrite { get; set; }
        public Boolean PermShare { get; set; }

        // Navigation
        //[ForeignKey("Username")]
        public virtual User User { get; set; }
        public virtual Folder Folder { get; set; }

    }

    #region Configuration
    public class PermissionConfiguration : EntityTypeConfiguration<Permission>
    {
        public PermissionConfiguration()
        {
            Property(p => p.Username).IsRequired();
            Property(p => p.FolderId).IsRequired();
            Property(p => p.IsOwner).IsRequired();
            Property(p => p.PermRead).IsRequired();
            Property(p => p.PermWrite).IsRequired();
            Property(p => p.PermShare).IsRequired();
        }
    }
    #endregion

}
