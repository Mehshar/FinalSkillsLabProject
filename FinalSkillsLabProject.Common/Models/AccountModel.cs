using System.ComponentModel.DataAnnotations;

namespace FinalSkillsLabProject.Common.Models
{
    public class AccountModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public int UserId { get; set; }
    }
}