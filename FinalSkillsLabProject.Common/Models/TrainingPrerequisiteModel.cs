using System.ComponentModel.DataAnnotations;

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