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
        private const string _AddUserQuery =
            @"BEGIN TRANSACTION;

            DECLARE @key int
            DECLARE @salt UNIQUEIDENTIFIER=NEWID()

            INSERT INTO [dbo].[EndUser] ([NIC], [FirstName], [LastName], [Email], [MobileNum], [Manager], [DepartmentId], [RoleId])
            SELECT @NIC, @FirstName, @LastName, @Email, @MobileNum, @Manager, @DepartmentId, @RoleId;

            SELECT @key = @@IDENTITY

            DELETE FROM [dbo].[PendingUser]
            WHERE [NIC] = @NIC;

            INSERT INTO [dbo].[Account] ([Username], [UserId], [Password], [Salt])
            SELECT @Username, @key, HASHBYTES('SHA2_512', @Password+CAST(@salt AS NVARCHAR(36))), @salt;

            COMMIT;";

        private const string _UpdateUserQuery =
            @"BEGIN TRANSACTION;

            UPDATE [dbo].[EndUser]
            SET [NIC] = @NIC,
	            [FirstName] = @FirstName,
	            [LastName] = @LastName,
	            [Email] = @Email,
	            [MobileNum] = @MobileNum,
	            [DepartmentId] = @DepartmentId,
	            [Manager] = @Manager,
	            [RoleId] = @RoleId
            WHERE [UserId] = @UserId;

            COMMIT;";

        private const string _DeleteUserQuery =
            @"BEGIN TRANSACTION;

            DELETE FROM [dbo].[EndUser]
            WHERE [UserId] = @UserId;

            COMMIT;";

        private const string _GetUserQuery =
            @"BEGIN TRANSACTION;

            SELECT *
            FROM [dbo].[EndUser]
            WHERE [UserId] = @UserId;

            COMMIT;";

        private const string _GetAllUsersQuery =
            @"BEGIN TRANSACTION;

            SELECT *
            FROM [dbo].[EndUser];

            COMMIT;";

        public bool Add(SignUpModel model)
        {
            List<SqlParameter> parameters = new List<SqlParameter>()
            {
                new SqlParameter("@NIC", model.NIC),
                new SqlParameter("@FirstName", model.FirstName),
                new SqlParameter("@LastName", model.LastName),
                new SqlParameter("@Email", model.Email),
                new SqlParameter("@MobileNum", model.MobileNum),
                new SqlParameter("@Manager", model.Manager),
                new SqlParameter("@DepartmentId", model.DepartmentId),
                new SqlParameter("@RoleId", (int)RoleEnum.Employee),
                new SqlParameter("@Username", model.Username),
                new SqlParameter("@Password", model.Password)
            };

            return DbCommand.InsertUpdateData(_AddUserQuery, parameters) > 0;
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
                new SqlParameter("@Manager", user.Manager),
                new SqlParameter("@RoleId", user.RoleId),
                new SqlParameter("@UserId", user.UserId)
            };
            return DbCommand.InsertUpdateData(_UpdateUserQuery, parameters) > 0;
        }

        public bool Delete(int userId)
        {
            SqlParameter parameter = new SqlParameter("@UserId", userId);

            return DbCommand.DeleteData(_DeleteUserQuery, parameter) > 0;
        }

        public UserModel Get(int userId)
        {
            UserModel user = null;
            List<SqlParameter> parameters = new List<SqlParameter>()
            {
                new SqlParameter("@UserId", userId)
            };

            DataTable dt = DbCommand.GetDataWithConditions(_GetUserQuery, parameters);

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
                    Manager = row["Manager"].ToString(),
                    RoleId = int.Parse(row["RoleId"].ToString())
                };
            }
            return user;
        }

        public IEnumerable<UserModel> GetAll()
        {
            DataTable dt = DbCommand.GetData(_GetAllUsersQuery);

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
                    Manager = row["Manager"].ToString(),
                    RoleId = int.Parse(row["RoleId"].ToString())
                };
                usersList.Add(user);
            }
            return usersList;
        }
    }
}
