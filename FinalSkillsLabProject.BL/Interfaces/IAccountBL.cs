using FinalSkillsLabProject.Common.Models;
using System.Threading.Tasks;

namespace FinalSkillsLabProject.BL.Interfaces
{
    public interface IAccountBL
    {
        Task<bool> AuthenticateUserAsync(LoginModel model);
        Task<UserViewModel> GetByUsernameAsync(string username);
        Task<string> UpdateAsync(AccountModel account);
    }
}
