﻿using System.ComponentModel.DataAnnotations;

namespace FinalSkillsLabProject.Common.Models
{
    public class UserModel
    {
        [Required]
        public int UserId { get; set; }

        [Required]
        [MinLength(14)]
        public string NIC { get; set; }

        [Required]
        [MinLength(3)]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [MinLength(3)]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        [RegularExpression(@"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$", ErrorMessage = "Invalid Email.")]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Mobile Number")]
        public string MobileNum { get; set; }

        [Required]
        public int DepartmentId { get; set; }
        public int? ManagerId { get; set; }

        [Required]
        public int RoleId { get; set; }
    }
}