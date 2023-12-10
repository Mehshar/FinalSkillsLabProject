using FinalSkillsLabProject.Common.Models;

namespace FinalSkillsLabProject.DAL.Interfaces
{
    public interface IAccountDAL
    {
        bool AuthenticateUser(LoginModel model);
        LoginModel GetByUsername(string username);
        AccountModel GetByUsernameAndUserId(string username, int userId);
        bool Update(AccountModel account);
    }
}
