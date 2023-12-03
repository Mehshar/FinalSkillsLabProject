using System;
using System.Collections.Generic;
using FinalSkillsLabProject.DAL.Common;
using System.Data.SqlClient;
using System.Data;
using FinalSkillsLabProject.Common.Models;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FinalSkillsLabProject.DAL.Interfaces;
using FinalSkillsLabProject.Common.Enums;

namespace FinalSkillsLabProject.DAL.DataAccessLayer
{
    public class DepartmentDAL : IDepartmentDAL
    {
        public bool Add(DepartmentModel department)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();

            parameters.Add(new SqlParameter("@DepartmentName", department.DepartmentName));

            const string InsertDepartmentQuery =
                @"BEGIN TRANSACTION;

                DECLARE @department_key INT            

                INSERT INTO [dbo].[Department] ([DepartmentName])
                SELECT @DepartmentName;

                SELECT @department_key = @@IDENTITY

                COMMIT;";

            return DbCommand.InsertUpdateData(InsertDepartmentQuery, parameters) > 0;
        }
        public bool Update(DepartmentModel department)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();

            parameters.Add(new SqlParameter("@DepartmentId", department.DepartmentId));
            parameters.Add(new SqlParameter("@DepartmentName", department.DepartmentName));

            const string UpdateDepartmentQuery =
                @"BEGIN TRANSACTION;

                UPDATE [dbo].[Department]
                SET [DepartmentName] = @DepartmentName
                WHERE [DepartmentId] = @DepartmentId;

                COMMIT;";

            return DbCommand.InsertUpdateData(UpdateDepartmentQuery, parameters) > 0;
        }
        //public void Delete(DepartmentModel department) { }
        public DepartmentModel Get(int departmentId)
        {
            DepartmentModel department = new DepartmentModel();

            List<SqlParameter> parameters = new List<SqlParameter>();

            parameters.Add(new SqlParameter("@DepartmentId", departmentId));

            const string GetDepartmentQuery =
                @"BEGIN TRANSACTION;
            
                SELECT *
                FROM [dbo].[Department]
                WHERE [DepartmentId] = @DepartmentId;

                COMMIT;";

            DataTable dt = DbCommand.GetDataWithConditions(GetDepartmentQuery, parameters);

            DataRow row = dt.Rows[0];
            department.DepartmentId = departmentId;
            department.DepartmentName = row["DepartmentName"].ToString();
            return department;
        }
        public IEnumerable<DepartmentModel> GetAll()
        {
            DepartmentModel department;

            const string GetAllDepartmentsQuery =
                @"BEGIN TRANSACTION;

                SELECT *
                FROM [dbo].[Department] 

                COMMIT;";

            DataTable dt = DbCommand.GetData(GetAllDepartmentsQuery);

            List<DepartmentModel> departmentsList = new List<DepartmentModel>();

            foreach (DataRow row in dt.Rows)
            {
                department = new DepartmentModel();

                department.DepartmentId = int.Parse(row["DepartmentId"].ToString());
                department.DepartmentName = row["DepartmentName"].ToString();

                departmentsList.Add(department);
            }
            return departmentsList;
        }

        public IEnumerable<UserModel> GetManagerByDepartment(int departmentId)
        {
            const string GetManagerByDepartmentQuery =
                @"SELECT [UserId], [FirstName], [LastName]
                FROM [dbo].[EndUser]
                WHERE [DepartmentId] = @DepartmentId AND [RoleId] = @RoleId";

            List<SqlParameter> parameters = new List<SqlParameter>()
            {
                new SqlParameter("DepartmentId", departmentId),
                new SqlParameter("RoleId", (int)RoleEnum.Manager)
            };

            DataTable dt = DbCommand.GetDataWithConditions(GetManagerByDepartmentQuery, parameters);
            UserModel user;
            List<UserModel> managersList = new List<UserModel>();
            foreach (DataRow row in dt.Rows)
            {
                user = new UserModel()
                {
                    UserId = int.Parse(row["UserId"].ToString()),
                    FirstName = row["FirstName"].ToString(),
                    LastName = row["LastName"].ToString(),
                    DepartmentId = departmentId
                };
                managersList.Add(user);
            }
            return managersList;
        }
    }
}
