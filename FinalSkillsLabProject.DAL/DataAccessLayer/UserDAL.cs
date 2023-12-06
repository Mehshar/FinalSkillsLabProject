using FinalSkillsLabProject.Common.Enums;
using System;
using System.Collections.Generic;
using FinalSkillsLabProject.DAL.Common;
using FinalSkillsLabProject.Common.Models;
using FinalSkillsLabProject.DAL.Interfaces;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalSkillsLabProject.DAL.DataAccessLayer
{
    public class UserDAL : IUserDAL
    {
        public bool Add(SignUpModel model)
        {
            List<SqlParameter> parameters = new List<SqlParameter>()
            {
                new SqlParameter("@NIC", model.NIC),
                new SqlParameter("@FirstName", model.FirstName),
                new SqlParameter("@LastName", model.LastName),
                new SqlParameter("@Email", model.Email),
                new SqlParameter("@MobileNum", model.MobileNum),
                new SqlParameter("@ManagerId", Convert.ToInt16(model.ManagerId)),
                new SqlParameter("@DepartmentId", Convert.ToInt16(model.DepartmentId)),
                new SqlParameter("@RoleId", Convert.ToInt16((int)RoleEnum.Employee)),
                new SqlParameter("@Username", model.Username),
                new SqlParameter("@Password", model.Password)
            };

            const string AddUserQuery =
                @"BEGIN TRANSACTION;

                DECLARE @key int
                DECLARE @salt UNIQUEIDENTIFIER=NEWID()

                INSERT INTO [dbo].[EndUser] ([NIC], [FirstName], [LastName], [Email], [MobileNum], [ManagerId], [DepartmentId], [RoleId])
                SELECT @NIC, @FirstName, @LastName, @Email, @MobileNum, @ManagerId, @DepartmentId, @RoleId;

                SELECT @key = @@IDENTITY

                INSERT INTO [dbo].[Account] ([Username], [UserId], [Password], [Salt])
                SELECT @Username, @key, HASHBYTES('SHA2_512', @Password+CAST(@salt AS NVARCHAR(36))), @salt;

                COMMIT;";

            return DbCommand.InsertUpdateData(AddUserQuery, parameters) > 0;
        }

        public bool Update(UserModel user)
        {
            List<SqlParameter> parameters = new List<SqlParameter>()
            {
                new SqlParameter("@NIC", user.NIC),
                new SqlParameter("@FirstName", user.FirstName),
                new SqlParameter("@LastName", user.LastName),
                new SqlParameter("@Email", user.Email),
                new SqlParameter("@MobileNum", user.MobileNum),
                new SqlParameter("@DepartmentId", user.DepartmentId),
                new SqlParameter("@ManagerId", user.ManagerId),
                new SqlParameter("@RoleId", user.RoleId),
                new SqlParameter("@UserId", user.UserId)
            };

            const string UpdateUserQuery =
              @"UPDATE [dbo].[EndUser]
                SET [NIC] = @NIC,
	                [FirstName] = @FirstName,
	                [LastName] = @LastName,
	                [Email] = @Email,
	                [MobileNum] = @MobileNum,
	                [DepartmentId] = @DepartmentId,
	                [ManagerId] = @ManagerId,
	                [RoleId] = @RoleId
                WHERE [UserId] = @UserId;";

            return DbCommand.InsertUpdateData(UpdateUserQuery, parameters) > 0;
        }

        public bool Delete(int userId)
        {
            SqlParameter parameter = new SqlParameter("@UserId", userId);

            const string DeleteUserQuery =
              @"DELETE FROM [dbo].[EndUser]
                WHERE [UserId] = @UserId;";

            return DbCommand.DeleteData(DeleteUserQuery, parameter) > 0;
        }

        public UserModel Get(int userId)
        {
            UserModel user = null;
            List<SqlParameter> parameters = new List<SqlParameter>()
            {
                new SqlParameter("@UserId", userId)
            };

            const string GetUserQuery =
              @"SELECT *
                FROM [dbo].[EndUser]
                WHERE [UserId] = @UserId;";

            DataTable dt = DbCommand.GetDataWithConditions(GetUserQuery, parameters);

            if (dt.Rows.Count > 0)
            {
                DataRow row = dt.Rows[0];
                user = new UserModel()
                {
                    UserId = userId,
                    NIC = row["NIC"].ToString(),
                    FirstName = row["FirstName"].ToString(),
                    LastName = row["LastName"].ToString(),
                    Email = row["Email"].ToString(),
                    MobileNum = row["MobileNum"].ToString(),
                    DepartmentId = int.Parse(row["DepartmentId"].ToString()),
                    ManagerId = int.TryParse(row["ManagerId"].ToString(), out int managerIdResult) ? (int?)managerIdResult : null,
                    RoleId = int.Parse(row["RoleId"].ToString())
                };
            }
            return user;
        }

        public IEnumerable<UserModel> GetAll()
        {
            const string GetAllUsersQuery =
              @"SELECT *
                FROM [dbo].[EndUser];";

            DataTable dt = DbCommand.GetData(GetAllUsersQuery);

            UserModel user;
            List<UserModel> usersList = new List<UserModel>();

            foreach (DataRow row in dt.Rows)
            {
                user = new UserModel()
                {
                    UserId = int.Parse(row["UserId"].ToString()),
                    NIC = row["NIC"].ToString(),
                    FirstName = row["FirstName"].ToString(),
                    LastName = row["LastName"].ToString(),
                    Email = row["Email"].ToString(),
                    MobileNum = row["MobileNum"].ToString(),
                    DepartmentId = int.Parse(row["DepartmentId"].ToString()),
                    ManagerId = int.TryParse(row["ManagerId"].ToString(), out int managerIdResult) ? (int?)managerIdResult : null,
                    RoleId = int.Parse(row["RoleId"].ToString())
                };
                usersList.Add(user);

            }
            return usersList;
        }
    }
}
