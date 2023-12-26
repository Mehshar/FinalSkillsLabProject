using System;
using System.ComponentModel.DataAnnotations;

namespace FinalSkillsLabProject.Common.Models
{
    public class TrainingModel
    {
        [Required]
        public int TrainingId { get; set; }

        [Required]
        [Display(Name = "Training Name")]
        public string TrainingName { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime Deadline { get; set; }
        public int Capacity { get; set; }
        public int? PriorityDepartment { get; set; }
        public string PriorityDepartmentName { get; set; }
    }
}