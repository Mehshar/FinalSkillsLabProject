using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalSkillsLabProject.Common.Models
{
    public class TrainingPrerequisiteViewModel
    {
        public int TrainingId { get; set; }
        public string TrainingName { get; set; }
        public string Description { get; set; }
        public DateTime Deadline { get; set; }
        public int Capacity { get; set; }
        public bool IsDeleted { get; set; }
        public int? PriorityDepartment { get; set; }
        public string PriorityDepartmentName { get; set; }
        public List<int> PrerequisiteIds { get; set; }
    }
}
