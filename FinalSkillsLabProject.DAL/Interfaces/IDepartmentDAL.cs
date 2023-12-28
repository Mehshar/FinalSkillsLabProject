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
        DepartmentModel Get(int departmentId);
        IEnumerable<DepartmentModel> GetAll();
        IEnumerable<UserModel> GetManagerByDepartment(int departmentId);
    }

}
