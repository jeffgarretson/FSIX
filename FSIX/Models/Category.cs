using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FSIX.Models
{
    public partial class Category
    {
        public Category()
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
