using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace FSIX.Models
{
    public partial class Item
    {
        // PK
        public int Id { get; set; }

        // Properties
        public string Name { get; set; }
        public string Description { get; set; }
        public string MIMEType { get; set; }
        public byte[] Content { get; set; }

        // Navigation
        public int FolderId { get; set; }
        public virtual Folder Folder { get; set; }
    }

    #region Configuration
    public class ItemConfiguration : EntityTypeConfiguration<Item>
    {
        public ItemConfiguration()
        {
            Property(i => i.Name).IsRequired();
        }
    }
    #endregion

}
