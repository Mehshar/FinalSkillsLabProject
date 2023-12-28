using System.Collections.Generic;
using System.Threading.Tasks;
using FinalSkillsLabProject.Common.Models;

namespace FinalSkillsLabProject.DAL.Interfaces
{
    public interface IDepartmentDAL
    {
        Task<bool> AddAsync(DepartmentModel department);
        Task<bool>UpdateAsync(DepartmentModel department);
        //void Delete(DepartmentModel department);
        Task<DepartmentModel> GetAsync(int departmentId);
        Task<IEnumerable<DepartmentModel>> GetAllAsync();
        Task<IEnumerable<UserModel>> GetManagerByDepartmentAsync(int departmentId);
    }

}
