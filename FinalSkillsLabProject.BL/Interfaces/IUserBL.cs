using System.Collections.Generic;
using System.Threading.Tasks;
using FinalSkillsLabProject.Common.Models;

namespace FinalSkillsLabProject.BL.Interfaces
{
    public interface IUserBL
    {
        Task<string> AddAsync(SignUpModel model);
        Task<string> UpdateAsync(UserModel user);
        Task<bool> DeleteAsync(int userId);
        Task<UserModel> GetAsync(int userId);
        Task<IEnumerable<UserModel>> GetAllAsync();
    }
}
