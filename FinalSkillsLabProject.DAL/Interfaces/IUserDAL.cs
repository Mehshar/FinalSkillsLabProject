using System.Collections.Generic;
using System.Threading.Tasks;
using FinalSkillsLabProject.Common.Models;

namespace FinalSkillsLabProject.DAL.Interfaces
{
    public interface IUserDAL
    {
        Task<bool> AddAsync(SignUpModel model);
        Task<bool> UpdateAsync(UserModel user);
        bool Delete(int userId);
        UserModel Get(int userId);
        IEnumerable<UserModel> GetAll();
    }
}
