using System.ComponentModel.DataAnnotations;
using FinalSkillsLabProject.Common.Enums;

namespace FinalSkillsLabProject.Common.Models
{
    public class RoleModel
    {
        [Required]
        public int RoleId { get; set; }

        [Required]
        public RoleEnum RoleName { get; set; }
    }
}