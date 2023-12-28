using FinalSkillsLabProject.BL.Interfaces;
using FinalSkillsLabProject.Common.Exceptions;
using FinalSkillsLabProject.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using FinalSkillsLabProject.Common.Models;
using System.Threading.Tasks;

namespace FinalSkillsLabProject.BL.BusinessLogicLayer
{
    public class DepartmentBL : IDepartmentBL
    {
        private readonly IDepartmentDAL _departmentDAL;

        public DepartmentBL(IDepartmentDAL departmentDAL)
        {
            this._departmentDAL = departmentDAL;
        }

        public async Task<string> AddAsync(DepartmentModel department)
        {
            try
            {
                DepartmentModel dept = (await GetAllAsync()).FirstOrDefault(x => x.DepartmentName.Equals(department.DepartmentName));
                CheckInsertUpdateDuplicate(dept);
                await this._departmentDAL.AddAsync(department);
                return "Department created successfully!";
            }

            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public async Task<DepartmentModel> GetAsync(int departmentId)
        {
            return await this._departmentDAL.GetAsync(departmentId);
        }

        public async Task<IEnumerable<DepartmentModel>> GetAllAsync()
        {
            return await this._departmentDAL.GetAllAsync();
        }

        public async Task<IEnumerable<UserModel>> GetManagerByDepartmentAsync(int departmentId)
        {
            return await this._departmentDAL.GetManagerByDepartmentAsync(departmentId);
        }

        public async Task<string> UpdateAsync(DepartmentModel department)
        {
            try
            {
                DepartmentModel dept = (await GetAllAsync())
                    .Where(x => x.DepartmentId != department.DepartmentId)
                    .FirstOrDefault(x => x.DepartmentName.Equals(department.DepartmentName));
                CheckInsertUpdateDuplicate(dept);
                await this._departmentDAL.UpdateAsync(department);
                return "Department updated successfully!";
            }

            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        private void CheckInsertUpdateDuplicate(DepartmentModel department)
        {
            string message = "";
            if (department != null)
            {
                message = "Department name already exists!";
                throw new DuplicationException(message);
            }
        }
    }
}
