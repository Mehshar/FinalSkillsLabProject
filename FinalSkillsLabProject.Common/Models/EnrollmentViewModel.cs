using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalSkillsLabProject.Common.Models
{
    public class EnrollmentViewModel
    {
        public int EnrollmentId { get; set; }
        public int EmployeeId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string EmployeeDepartment { get; set; }
        public DateTime EnrollmentDate { get; set; }
        public string TrainingName { get; set; }
        public int TrainingId { get; set; }
        public string PriorityDepartmentName { get; set; }
        public int Capacity { get; set; }
        public string EnrollmentStatus { get; set; }
    }
}
