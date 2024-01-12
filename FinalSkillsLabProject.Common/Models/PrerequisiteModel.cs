using System.ComponentModel.DataAnnotations;

namespace FinalSkillsLabProject.Common.Models
{
    public class PrerequisiteModel
    {
        public int PrerequisiteId { get; set; }
        public string Type { get; set; }
        public string Description { get; set; }
    }
}