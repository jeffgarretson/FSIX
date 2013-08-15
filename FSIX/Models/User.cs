using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace FSIX.Models
{
    public partial class User
    {
        public User()
        {
            this.Permissions = new HashSet<Permission>();
        }

        // PK
        public string Username { get; set; }

        // Properties
        public string DisplayName { get; set; }
        public string UserType { get; set; }        /* Values: Admin, User */

        // Navigation
        public virtual ICollection<Permission> Permissions { get; set; }
    }

    #region Configuration
    public class UserConfiguration : EntityTypeConfiguration<User>
    {
        public UserConfiguration()
        {
            HasKey(u => u.Username);
            Property(u => u.Username).HasMaxLength(100);
            Property(u => u.DisplayName).HasMaxLength(100);
            Property(u => u.UserType).HasMaxLength(5);
        }
    }
    #endregion

}
