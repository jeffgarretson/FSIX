using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FSIX.Models
{
    public partial class Severity
    {
        public Severity()
        {
            this.Logs = new HashSet<Log>();
        }

        // PK
        public int Id { get; set; }

        // Properties
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }

        // Navigation
        public virtual ICollection<Log> Logs { get; set; }
    }
}
