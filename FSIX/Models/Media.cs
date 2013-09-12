using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace FSIX.Models
{
    public partial class Media
    {
        // PK
        public int Id { get; set; }

        // Properties
        public string FileName { get; set; }
        public string MimeType { get; set; }
        public int Bytes { get; set; }
        public DateTime CreatedTime { get; internal set; }
        public DateTime ModifiedTime { get; internal set; }
        public DateTime ExpirationTime { get; internal set; }

        // Navigation
        public int ItemId { get; set; }
        public virtual Item Item { get; set; }

        public string SubmittedByUsername { get; internal set; }
        public virtual User SubmittedBy { get; internal set; }

        public virtual MediaInternalData MediaInternalData { get; internal set; }
    }

    public partial class MediaInternalData
    {
        public int Id { get; set; }
        public string LocalFileName { get; internal set; }
        public byte[] IV { get; internal set; }
    }

    #region Configuration

    public class MediaConfiguration : EntityTypeConfiguration<Media>
    {
        public MediaConfiguration()
        {
            // Explicitly define table name (for "table splitting" and because "Media" is inherently plural)
            ToTable("Media");

            // FK to SubmittedBy User
            HasRequired(m => m.SubmittedBy)
                .WithMany()    // Do not create User => Media relationship
                .HasForeignKey(m => m.SubmittedByUsername)
                .WillCascadeOnDelete(false);

            // Shared primary key with MediaInternalData ("table splitting")
            HasRequired(m => m.MediaInternalData)
                .WithRequiredPrincipal();

            // Properties
            Property(m => m.FileName).IsRequired();
            Property(m => m.MimeType).IsRequired();
            Property(m => m.Bytes).IsRequired();
            Property(m => m.CreatedTime).IsRequired();
            Property(m => m.ModifiedTime).IsRequired();
            Property(m => m.ExpirationTime).IsRequired();
        }
    }

    public class MediaInternalDataConfiguration : EntityTypeConfiguration<MediaInternalData>
    {
        public MediaInternalDataConfiguration()
        {
            // This entity shares the Media table ("table splitting")
            ToTable("Media");

            // Properties
            Property(m => m.LocalFileName).IsRequired();
            Property(m => m.IV).IsRequired();
        }
    }
    
    #endregion

}
