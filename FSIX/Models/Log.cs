using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FSIX.Models
{
    public partial class Log
    {
        // PK
        public int Id { get; set; }

        // Properties
        [Required]
        public System.DateTime Timestamp { get; set; }

        [Required]
        public string Message { get; set; }

        // Navigation
        public int CategoryId { get; set; }
        public virtual Category Category { get; set; }

        public int SeverityId { get; set; }
        public virtual Severity Severity { get; set; }

        public Nullable<int> FolderId { get; set; }
        public virtual Folder Folder { get; set; }

        public string Username { get; set; }
        public virtual User User { get; set; }
    }
}
