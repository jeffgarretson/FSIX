using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FSIX.Models
{
    public partial class Configuration
    {
        // PK
        public int Id { get; set; }

        // Properties
        [Required]
        public string Name { get; set; }
        public string Value { get; set; }
        public string Description { get; set; }
    }
}
