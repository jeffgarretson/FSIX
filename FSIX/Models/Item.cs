using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace FSIX.Models
{
    public partial class Item
    {
        public Item()
        {
            this.Media = new HashSet<Media>();
        }

        // PK
        public int Id { get; set; }

        // Properties
        public string Type { get; set; }    // { "Note", "File", "Image" }
        public string Note { get; set; }
        public DateTime CreatedTime { get; internal set; }
        public DateTime ModifiedTime { get; internal set; }

        // Navigation
        public int FolderId { get; set; }
        public virtual Folder Folder { get; set; }

        public string CreatedByUsername { get; internal set; }
        public virtual User CreatedBy { get; internal set; }

        public virtual ICollection<Media> Media { get; set; }
    }

    #region Configuration
    public class ItemConfiguration : EntityTypeConfiguration<Item>
    {
        public ItemConfiguration ()
        {
            // FK to CreatedBy User
            HasRequired(i => i.CreatedBy)
                .WithMany()    // Do not create User => Items relationship
                .HasForeignKey(i => i.CreatedByUsername);

            // Properties
            Property(i => i.CreatedTime).IsRequired();
            Property(i => i.ModifiedTime).IsRequired();
            Property(i => i.Type).IsRequired().HasMaxLength(10);
        }
    }
    #endregion

}
