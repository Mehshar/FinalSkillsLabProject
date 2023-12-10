using System.ComponentModel.DataAnnotations;

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
