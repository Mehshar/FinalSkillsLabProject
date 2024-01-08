using System.Collections.Generic;
using FinalSkillsLabProject.DAL.Common;
using System.Data.SqlClient;
using FinalSkillsLabProject.Common.Models;
using FinalSkillsLabProject.DAL.Interfaces;
using FinalSkillsLabProject.Common.Enums;
using System.Threading.Tasks;

namespace FinalSkillsLabProject.DAL.DataAccessLayer
{
    public class DepartmentDAL : IDepartmentDAL
    {
        public async Task<bool> AddAsync(DepartmentModel department)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();

            parameters.Add(new SqlParameter("@DepartmentName", department.DepartmentName));

            const string InsertDepartmentQuery =
              @"DECLARE @department_key INT            

                INSERT INTO [dbo].[Department] ([DepartmentName])
                SELECT @DepartmentName;

                SELECT @department_key = @@IDENTITY";

            return await DbCommand.InsertUpdateDataAsync(InsertDepartmentQuery, parameters) > 0;
        }
        public async Task<bool> UpdateAsync(DepartmentModel department)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();

            parameters.Add(new SqlParameter("@DepartmentId", department.DepartmentId));
            parameters.Add(new SqlParameter("@DepartmentName", department.DepartmentName));

            const string UpdateDepartmentQuery =
              @"UPDATE [dbo].[Department]
                SET [DepartmentName] = @DepartmentName
                WHERE [DepartmentId] = @DepartmentId;";

            return await DbCommand.InsertUpdateDataAsync(UpdateDepartmentQuery, parameters) > 0;
        }
        //public void Delete(DepartmentModel department) { }
        public async Task<DepartmentModel> GetAsync(int departmentId)
        {
            DepartmentModel department = null;
            List<SqlParameter> parameters = new List<SqlParameter>();

            parameters.Add(new SqlParameter("@DepartmentId", departmentId));

            const string GetDepartmentQuery =
              @"SELECT *
                FROM [dbo].[Department]
                WHERE [DepartmentId] = @DepartmentId;";

            using (SqlDataReader reader = await DbCommand.GetDataWithConditionsAsync(GetDepartmentQuery, parameters))
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

        public async Task<IEnumerable<DepartmentModel>> GetAllAsync()
        {
            DepartmentModel department;
            List<DepartmentModel> departmentsList = new List<DepartmentModel>();

            const string GetAllDepartmentsQuery =
              @"SELECT *
                FROM [dbo].[Department];";

            using (SqlDataReader reader = await DbCommand.GetDataAsync(GetAllDepartmentsQuery))
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

        public async Task<IEnumerable<UserModel>> GetManagerByDepartmentAsync(int departmentId)
        {
            const string GetManagerByDepartmentQuery =
              @"SELECT eu.[UserId], eu.[FirstName], eu.[LastName]
                FROM [dbo].[EndUser] eu
                INNER JOIN [dbo].[RoleAssignment] ra
                ON eu.[UserId] = ra.[UserId]
                WHERE [DepartmentId] = @DepartmentId AND [RoleId] = @RoleId";

            List<SqlParameter> parameters = new List<SqlParameter>()
            {
                new SqlParameter("DepartmentId", departmentId),
                new SqlParameter("RoleId", (int)RoleEnum.Manager)
            };

            UserModel user;
            List<UserModel> managersList = new List<UserModel>();

            using (SqlDataReader reader = await DbCommand.GetDataWithConditionsAsync(GetManagerByDepartmentQuery, parameters))
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
