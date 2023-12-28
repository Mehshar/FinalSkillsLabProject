using FinalSkillsLabProject.Common.Models;
using System.Threading.Tasks;

namespace FinalSkillsLabProject.DAL.Interfaces
{
    public interface IAccountDAL
    {
        bool AuthenticateUser(LoginModel model);
        UserViewModel GetByUsername(string username);
        AccountModel GetByUsernameAndUserId(string username, int userId);
        Task<bool> UpdateAsync(AccountModel account);
    }
}
