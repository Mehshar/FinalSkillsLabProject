using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FinalSkillsLabProject.Common.Models
{
    public class PrerequisiteModel
    {
        [Required]
        public int PrerequisiteId { get; set; }

        [Required]
        public string Type { get; set; }

        [Required]
        public string Description { get; set; }
    }
}