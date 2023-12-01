﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FinalSkillsLabProject.Common.Models
{
    public class EnrollmentModel
    {
        [Required]
        public int UserId { get; set; }

        [Required]
        public int TrainingId { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        [Display(Name = "Enrollment Date")]
        public DateTime EnrollmentDate { get; set; }

        [Required]
        [Display(Name = "Enrollment Status")]
        public string EnrollmentStatus { get; set; }

        public string PrerequisiteMaterial { get; set; } // subject to change

        [Required]
        [Display(Name = "Reason for Decline")]
        public string DeclineReason { get; set; } = "N/A";
    }
}