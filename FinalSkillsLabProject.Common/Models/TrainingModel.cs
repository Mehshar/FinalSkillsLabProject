using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

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
        public int PriorityDepartment { get; set; }
        public string PriorityDepartmentName { get; set; }
    }
}