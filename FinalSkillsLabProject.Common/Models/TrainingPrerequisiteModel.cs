using System.ComponentModel.DataAnnotations;

namespace FinalSkillsLabProject.Common.Models
{
    public class TrainingPrerequisiteModel
    {
        public int TrainingId { get; set; }
        public int PrerequisiteId { get; set; }
    }
}