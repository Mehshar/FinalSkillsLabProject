using FinalSkillsLabProject.Common.Models;

namespace FinalSkillsLabProject.BL.Interfaces
{
    public interface IAccountBL
    {
        bool AuthenticateUser(LoginModel model);
        LoginModel GetByUsername(string username);
    }
}
