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
        DepartmentModel Get(int departmentId);
        IEnumerable<DepartmentModel> GetAll();
        IEnumerable<UserModel> GetManagerByDepartment(int departmentId);
    }
}
