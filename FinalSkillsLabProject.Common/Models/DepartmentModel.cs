using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FinalSkillsLabProject.Common.Models
{
    public class DepartmentModel
    {
        [Required]
        public int DepartmentId { get; set; }

        [Required]
        [Display(Name = "Department Name")]
        public string DepartmentName { get; set; }
    }
}