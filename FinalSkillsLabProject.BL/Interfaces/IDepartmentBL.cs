using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FinalSkillsLabProject.Common;
using FinalSkillsLabProject.Common.Models;

namespace FinalSkillsLabProject.BL.Interfaces
{
    public interface IDepartmentBL
    {
        string Add(DepartmentModel department);
        string Update(DepartmentModel department);
        //void Delete(DepartmentModel department);
        DepartmentModel Get(int departmentId);
        IEnumerable<DepartmentModel> GetAll();
        IEnumerable<UserModel> GetManagerByDepartment(int departmentId);
    }
}
