namespace FinalSkillsLabProject.Common.Models
{
    public class LoginModel
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public RoleModel Role { get; set; }
    }
}