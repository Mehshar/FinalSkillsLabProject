using System;

namespace FinalSkillsLabProject.Common.Models
{
    public class EnrollmentSelectionViewModel
    {
        public string TrainingName { get; set; }
        public DateTime Deadline { get; set; }
        public string EmployeeEmail { get; set; }
        public string EmployeeUsername { get; set; }
        public string ManagerEmail { get; set; }
        public string ManagerFirstName { get; set; }
        public string ManagerLastName { get; set; }
    }
}
