using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FinalSkillsLabProject.Common.Models
{
    public class TrainingPrerequisiteModel
    {
        [Required]
        public int TrainingId { get; set; }

        [Required]
        public int PrerequisiteId { get; set; }
    }
}