using System.Collections.Generic;
using FinalSkillsLabProject.DAL.Common;
using System.Data.SqlClient;
using FinalSkillsLabProject.Common.Models;
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
              @"DECLARE @department_key INT            

                INSERT INTO [dbo].[Department] ([DepartmentName])
                SELECT @DepartmentName;

                SELECT @department_key = @@IDENTITY";

            return DbCommand.InsertUpdateData(InsertDepartmentQuery, parameters) > 0;
        }
        public bool Update(DepartmentModel department)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();

            parameters.Add(new SqlParameter("@DepartmentId", department.DepartmentId));
            parameters.Add(new SqlParameter("@DepartmentName", department.DepartmentName));

            const string UpdateDepartmentQuery =
              @"UPDATE [dbo].[Department]
                SET [DepartmentName] = @DepartmentName
                WHERE [DepartmentId] = @DepartmentId;";

            return DbCommand.InsertUpdateData(UpdateDepartmentQuery, parameters) > 0;
        }
        //public void Delete(DepartmentModel department) { }
        public DepartmentModel Get(int departmentId)
        {
            DepartmentModel department = null;
            List<SqlParameter> parameters = new List<SqlParameter>();

            parameters.Add(new SqlParameter("@DepartmentId", departmentId));

            const string GetDepartmentQuery =
              @"SELECT *
                FROM [dbo].[Department]
                WHERE [DepartmentId] = @DepartmentId;";

            using (SqlDataReader reader = DbCommand.GetDataWithConditions(GetDepartmentQuery, parameters))
            {
                if (reader.Read())
                {
                    department = new DepartmentModel()
                    {
                        DepartmentId = departmentId,
                        DepartmentName = reader.GetString(reader.GetOrdinal("DepartmentName"))
                    };
                }
            }
            return department;
        }

        public IEnumerable<DepartmentModel> GetAll()
        {
            DepartmentModel department;
            List<DepartmentModel> departmentsList = new List<DepartmentModel>();

            const string GetAllDepartmentsQuery =
              @"SELECT *
                FROM [dbo].[Department] ";

            using (SqlDataReader reader = DbCommand.GetData(GetAllDepartmentsQuery))
            {
                while (reader.Read())
                {
                    department = new DepartmentModel
                    {
                        DepartmentId = reader.GetInt16(reader.GetOrdinal("DepartmentId")),
                        DepartmentName = reader.GetString(reader.GetOrdinal("DepartmentName"))
                    };
                    departmentsList.Add(department);
                }
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

            UserModel user;
            List<UserModel> managersList = new List<UserModel>();

            using (SqlDataReader reader = DbCommand.GetDataWithConditions(GetManagerByDepartmentQuery, parameters))
            {
                while (reader.Read())
                {
                    user = new UserModel()
                    {
                        UserId = reader.GetInt16(reader.GetOrdinal("UserId")),
                        FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                        LastName = reader.GetString(reader.GetOrdinal("LastName")),
                        DepartmentId = departmentId
                    };
                    managersList.Add(user);
                }
            }
            return managersList;
        }
    }
}
