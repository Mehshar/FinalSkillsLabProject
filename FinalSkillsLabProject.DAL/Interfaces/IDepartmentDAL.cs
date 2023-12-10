using System.Collections.Generic;
using FinalSkillsLabProject.Common.Models;

namespace FinalSkillsLabProject.DAL.Interfaces
{
    public interface IDepartmentDAL
    {
        bool Add(DepartmentModel department);
        bool Update(DepartmentModel department);
        //void Delete(DepartmentModel department);
        DepartmentModel Get(int departmentId);
        IEnumerable<DepartmentModel> GetAll();
        IEnumerable<UserModel> GetManagerByDepartment(int departmentId);
    }

}
