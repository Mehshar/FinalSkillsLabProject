using System.Collections.Generic;
using System.Threading.Tasks;
using FinalSkillsLabProject.Common.Models;

namespace FinalSkillsLabProject.BL.Interfaces
{
    public interface IUserBL
    {
        Task<string> AddAsync(SignUpModel model);
        Task<string> UpdateAsync(UserModel user);
        bool Delete(int userId);
        UserModel Get(int userId);
        IEnumerable<UserModel> GetAll();
    }
}
