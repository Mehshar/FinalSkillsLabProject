using FinalSkillsLabProject.Common.Models;
using System.Threading.Tasks;

namespace FinalSkillsLabProject.BL.Interfaces
{
    public interface IAccountBL
    {
        bool AuthenticateUser(LoginModel model);
        UserViewModel GetByUsername(string username);
        Task<string> UpdateAsync(AccountModel account);
    }
}
