using System.ComponentModel.DataAnnotations;

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