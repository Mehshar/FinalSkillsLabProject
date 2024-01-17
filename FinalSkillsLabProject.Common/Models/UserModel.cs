namespace FinalSkillsLabProject.Common.Models
{
    public class UserModel
    {
        public int UserId { get; set; }
        public string NIC { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string MobileNum { get; set; }
        public int DepartmentId { get; set; }
        public int? ManagerId { get; set; }
        public int RoleId { get; set; }
    }
}