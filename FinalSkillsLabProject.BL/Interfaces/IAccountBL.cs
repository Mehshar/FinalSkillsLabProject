using FinalSkillsLabProject.Common.Models;

namespace FinalSkillsLabProject.BL.Interfaces
{
    public interface IAccountBL
    {
        bool AuthenticateUser(LoginModel model);
        UserViewModel GetByUsername(string username);
    }
}
