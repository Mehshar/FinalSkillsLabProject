using FinalSkillsLabProject.Common.Models;
using System.Threading.Tasks;

namespace FinalSkillsLabProject.DAL.Interfaces
{
    public interface IAccountDAL
    {
        //Task<bool> AuthenticateUserAsync(LoginModel model);
        Task<(byte[], byte[])> AuthenticateUserAsync(LoginModel model);
        Task<UserViewModel> GetByUsernameAsync(string username);
        Task<AccountModel> GetByUsernameAndUserIdAsync(string username, int userId);
        Task<bool> UpdateAsync(AccountModel account);
    }
}
