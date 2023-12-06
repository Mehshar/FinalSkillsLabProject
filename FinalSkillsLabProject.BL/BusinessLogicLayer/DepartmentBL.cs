using FinalSkillsLabProject.BL.Interfaces;
using FinalSkillsLabProject.Common.Exceptions;
using FinalSkillsLabProject.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using FinalSkillsLabProject.Common.Models;

namespace FinalSkillsLabProject.BL.BusinessLogicLayer
{
    public class DepartmentBL : IDepartmentBL
    {
        private readonly IDepartmentDAL _departmentDAL;

        public DepartmentBL(IDepartmentDAL departmentDAL)
        {
            this._departmentDAL = departmentDAL;
        }

        public string Add(DepartmentModel department)
        {
            try
            {
                DepartmentModel dept = GetAll().FirstOrDefault(x => x.DepartmentName.Equals(department.DepartmentName));
                CheckInsertUpdateDuplicate(dept);
                this._departmentDAL.Add(department);
                return "Department created successfully!";
            }

            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public DepartmentModel Get(int departmentId)
        {
            return this._departmentDAL.Get(departmentId);
        }

        public IEnumerable<DepartmentModel> GetAll()
        {
            return this._departmentDAL.GetAll();
        }

        public IEnumerable<UserModel> GetManagerByDepartment(int departmentId)
        {
            return this._departmentDAL.GetManagerByDepartment(departmentId);
        }

        public string Update(DepartmentModel department)
        {
            try
            {
                DepartmentModel dept = GetAll()
                    .Where(x => x.DepartmentId != department.DepartmentId)
                    .FirstOrDefault(x => x.DepartmentName.Equals(department.DepartmentName));
                CheckInsertUpdateDuplicate(dept);
                this._departmentDAL.Update(department);
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
