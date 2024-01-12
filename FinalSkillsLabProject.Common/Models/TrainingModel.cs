using System;
using System.ComponentModel.DataAnnotations;

namespace FinalSkillsLabProject.Common.Models
{
    public class TrainingModel
    {
        public int TrainingId { get; set; }
        public string TrainingName { get; set; }
        public string Description { get; set; }
        public DateTime Deadline { get; set; }
        public int Capacity { get; set; }
        public int? PriorityDepartment { get; set; }
        public string PriorityDepartmentName { get; set; }
        public bool IsDeleted { get; set; }
    }
}