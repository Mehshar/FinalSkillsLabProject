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
            const string AuthenticateUserQuery =
              @"SELECT euser.*, acc.*
                FROM [dbo].[EndUser] euser WITH(NOLOCK)
                INNER JOIN [dbo].[Account] acc WITH(NOLOCK)
                ON euser.[UserId] = acc.UserId
                WHERE acc.[Username] = @Username AND acc.[Password] = HASHBYTES('SHA2_512', @Password+CAST(Salt AS NVARCHAR(36)));";

            List<SqlParameter> parameters = new List<SqlParameter>()
            {
                new SqlParameter("@Username", model.Username),
                new SqlParameter("@Password", model.Password)
            };

            using (SqlDataReader reader = DbCommand.GetDataWithConditions(AuthenticateUserQuery, parameters))
            {
                return reader.HasRows;
            }
        }

        public LoginModel GetByUsername(string username)
        {
            LoginModel user = null;
            List<SqlParameter> parameters = new List<SqlParameter>()
            {
                new SqlParameter("@Username", username)
            };

            const string GetUserByUsernameQuery =
              @"SELECT euser.[UserId], euser.[RoleId], acc.[Username]
                FROM [dbo].[EndUser] euser WITH(NOLOCK)
                INNER JOIN [dbo].[Account] acc WITH(NOLOCK) ON euser.[UserId] = acc.[UserId]
                INNER JOIN [dbo].[Role] r WITH(NOLOCK) ON euser.[RoleId] = r.[RoleId]
                WHERE acc.[Username] = @Username;";

            using (SqlDataReader reader = DbCommand.GetDataWithConditions(GetUserByUsernameQuery, parameters))
            {
                if (reader.Read())
                {
                    user = new LoginModel()
                    {
                        UserId = reader.GetInt16(reader.GetOrdinal("UserId")),
                        Username = reader.GetString(reader.GetOrdinal("Username")),
                        Role = new RoleModel()
                        {
                            RoleId = reader.GetInt16(reader.GetOrdinal("RoleId")),
                            RoleName = (RoleEnum)reader.GetInt16(reader.GetOrdinal("RoleId"))
                        }
                    };
                }
            }
            return user;
        }

        // Getting a user with a different UserId, who has a specific username
        public AccountModel GetByUsernameAndUserId(string username, int userId)
        {
            AccountModel user = null;
            List<SqlParameter> parameters = new List<SqlParameter>()
            {
                new SqlParameter("@Username", username),
                new SqlParameter("@UserId", userId)
            };

            const string GetUserByUsernameAndUserId =
              @"SELECT euser.[UserId], acc.[Username], acc.[Password]
                FROM [dbo].[EndUser] euser WITH(NOLOCK)
                INNER JOIN [dbo].[Account] acc WITH(NOLOCK) ON euser.[UserId] = acc.[UserId]
                INNER JOIN [dbo].[Role] r WITH(NOLOCK) ON euser.[RoleId] = r.[RoleId]
                WHERE acc.[Username] = @Username AND euser.[UserId] != @UserId;";

            using (SqlDataReader reader = DbCommand.GetDataWithConditions(GetUserByUsernameAndUserId, parameters))
            {
                if (reader.Read())
                {
                    user = new AccountModel()
                    {
                        UserId = reader.GetInt16(reader.GetOrdinal("UserId")),
                        Username = reader.GetString(reader.GetOrdinal("Username")),
                        Password = reader.GetString(reader.GetOrdinal("Password"))
                    };
                }
            }
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
              @"UPDATE [dbo].[Account]
                SET [Username] = @Username,
	                [Password] = @Password
                WHERE [UserId] = @UserId;";

            return DbCommand.InsertUpdateData(UpdateAccountQuery, parameters) > 0;
        }
    }
}
