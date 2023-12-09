using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalSkillsLabProject.Common.Models
{
    public class PrerequisiteMaterialModel
    {
        public int PrerequisiteMaterialId { get; set; }

        [Required]
        public int EnrollmentId { get; set; }

        [Required]
        public int PrerequisiteId { get; set; }

        [Required]
        public string PrerequisiteMaterialURL { get; set; }
    }
}
