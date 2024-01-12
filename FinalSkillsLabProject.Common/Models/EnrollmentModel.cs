using System;
using System.ComponentModel.DataAnnotations;

namespace FinalSkillsLabProject.Common.Models
{
    public class EnrollmentModel
    {
        public int EnrollmentId { get; set; }
        public int UserId { get; set; }
        public int TrainingId { get; set; }
        public DateTime EnrollmentDate { get; set; }
        public string EnrollmentStatus { get; set; }
        public string DeclineReason { get; set; } = "N/A";
    }
}