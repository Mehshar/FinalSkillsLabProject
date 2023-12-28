using System.Collections.Generic;
using System.Threading.Tasks;
using FinalSkillsLabProject.Common.Models;

namespace FinalSkillsLabProject.BL.Interfaces
{
    public interface IDepartmentBL
    {
        Task<string> AddAsync(DepartmentModel department);
        Task<string> UpdateAsync(DepartmentModel department);
        //void Delete(DepartmentModel department);
        Task<DepartmentModel> GetAsync(int departmentId);
        Task<IEnumerable<DepartmentModel>> GetAllAsync();
        Task<IEnumerable<UserModel>> GetManagerByDepartmentAsync(int departmentId);
    }
}
