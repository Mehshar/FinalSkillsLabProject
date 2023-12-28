using System.Collections.Generic;
using System.Threading.Tasks;
using FinalSkillsLabProject.Common.Models;

namespace FinalSkillsLabProject.DAL.Interfaces
{
    public interface IUserDAL
    {
        Task<bool> AddAsync(SignUpModel model);
        Task<bool> UpdateAsync(UserModel user);
        Task<bool> DeleteAsync(int userId);
        Task<UserModel> GetAsync(int userId);
        Task<IEnumerable<UserModel>> GetAllAsync();
    }
}
