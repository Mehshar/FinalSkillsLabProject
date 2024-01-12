using System.ComponentModel.DataAnnotations;

namespace FinalSkillsLabProject.Common.Models
{
    public class PrerequisiteMaterialModel
    {
        public int PrerequisiteMaterialId { get; set; }
        public int EnrollmentId { get; set; }
        public int PrerequisiteId { get; set; }
        public string PrerequisiteMaterialURL { get; set; }
    }
}
