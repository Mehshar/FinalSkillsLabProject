using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalSkillsLabProject.Common.Models
{
    public class PrerequisiteMaterialViewModel
    {
        public int PrerequisiteMaterialId { get; set; }
        public int EnrollmentId { get; set; }
        public string EnrollmentStatus { get; set; }
        public int PrerequisiteId { get; set; }
        public string Description { get; set; }
        public string PrerequisiteMaterialURL { get; set; }
    }
}
