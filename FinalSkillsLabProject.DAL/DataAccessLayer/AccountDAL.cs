using FinalSkillsLabProject.Common.Enums;
using FinalSkillsLabProject.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FinalSkillsLabProject.Common.Models;
using FinalSkillsLabProject.DAL.Common;

namespace FinalSkillsLabProject.DAL.DataAccessLayer
{
    public class AccountDAL : IAccountDAL
    {
        public bool AuthenticateUser(LoginModel model)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();

            parameters.Add(new SqlParameter("@Username", model.Username));
            parameters.Add(new SqlParameter("@Password", model.Password));

            const string AuthenticateUserQuery =
                @"BEGIN TRANSACTION;

                SELECT euser.*, acc.*
                FROM [dbo].[EndUser] euser WITH(NOLOCK)
                INNER JOIN [dbo].[Account] acc WITH(NOLOCK)
                ON euser.[UserId] = acc.UserId
                WHERE acc.[Username] = @Username AND acc.[Password] = HASHBYTES('SHA2_512', @Password+CAST(Salt AS NVARCHAR(36)));   

                COMMIT;";

            DataTable dt = DbCommand.GetDataWithConditions(AuthenticateUserQuery, parameters);
            return dt.Rows.Count > 0;
        }

        public LoginModel GetByUsername(string username)
        {
            LoginModel user = null;
            List<SqlParameter> parameters = new List<SqlParameter>();

            parameters.Add(new SqlParameter("@Username", username));

            const string GetUserByUsernameQuery =
                @"BEGIN TRANSACTION; 

                SELECT euser.[UserId], euser.[RoleId], acc.[Username]
                FROM [dbo].[EndUser] euser WITH(NOLOCK)
                INNER JOIN [dbo].[Account] acc WITH(NOLOCK) ON euser.[UserId] = acc.[UserId]
                INNER JOIN [dbo].[Role] r WITH(NOLOCK) ON euser.[RoleId] = r.[RoleId]
                WHERE acc.[Username] = @Username;

                COMMIT;";

            DataTable dt = DbCommand.GetDataWithConditions(GetUserByUsernameQuery, parameters);

            if (dt.Rows.Count > 0)
            {
                DataRow row = dt.Rows[0];
                user = new LoginModel()
                {
                    UserId = int.Parse(row["UserId"].ToString()),
                    Username = row["Username"].ToString(),
                    Role = new RoleModel() { RoleId = int.Parse(row["RoleId"].ToString()), RoleName = (RoleEnum)(int.Parse(row["RoleId"].ToString())) }
                };
            }
            return user;
        }

        // Getting a user with a different UserId, who has a specific username
        public AccountModel GetByUsernameAndUserId(string username, int userId)
        {
            List<SqlParameter> parameters = new List<SqlParameter>()
            {
                new SqlParameter("@Username", username),
                new SqlParameter("@UserId", userId)
            };

            const string GetUserByUsernameAndUserId =
                @"BEGIN TRANSACTION;

                SELECT euser.[UserId], acc.[Username], acc.[Password]
                FROM [dbo].[EndUser] euser WITH(NOLOCK)
                INNER JOIN [dbo].[Account] acc WITH(NOLOCK) ON euser.[UserId] = acc.[UserId]
                INNER JOIN [dbo].[Role] r WITH(NOLOCK) ON euser.[RoleId] = r.[RoleId]
                WHERE acc.[Username] = @Username AND euser.[UserId] != @UserId;

                COMMIT;";

            DataTable dt = DbCommand.GetDataWithConditions(GetUserByUsernameAndUserId, parameters);

            DataRow row = dt.Rows[0];
            AccountModel user = row != null ? new AccountModel()
            {
                UserId = int.Parse(row["UserId"].ToString()),
                Username = row["Username"].ToString(),
                Password = row["Password"].ToString()
            } : null;
            return user;
        }

        public bool Update(AccountModel account)
        {
            List<SqlParameter> parameters = new List<SqlParameter>()
            {
                new SqlParameter("@Username", account.Username.Trim()),
                new SqlParameter("@Password", account.Password.Trim()),
                new SqlParameter("@UserId", account.UserId)
            };

            const string UpdateAccountQuery =
                @"BEGIN TRANSACTION;

                UPDATE [dbo].[Account]
                SET [Username] = @Username,
	                [Password] = @Password
                WHERE [UserId] = @UserId;

                COMMIT;";

            return DbCommand.InsertUpdateData(UpdateAccountQuery, parameters) > 0;
        }
    }
}
