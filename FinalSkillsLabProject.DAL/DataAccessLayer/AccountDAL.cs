using FinalSkillsLabProject.Common.Enums;
using FinalSkillsLabProject.DAL.Interfaces;
using System.Collections.Generic;
using System.Data.SqlClient;
using FinalSkillsLabProject.Common.Models;
using FinalSkillsLabProject.DAL.Common;
using System.Threading.Tasks;

namespace FinalSkillsLabProject.DAL.DataAccessLayer
{
    public class AccountDAL : IAccountDAL
    {
        public async Task<bool> AuthenticateUserAsync(LoginModel model)
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

            using (SqlDataReader reader = await DbCommand.GetDataWithConditionsAsync(AuthenticateUserQuery, parameters))
            {
                return reader.HasRows;
            }
        }

        public async Task<UserViewModel> GetByUsernameAsync(string username)
        {
            UserViewModel user = null;
            List<SqlParameter> parameters = new List<SqlParameter>()
            {
                new SqlParameter("@Username", username)
            };

            const string GetUserByUsernameQuery =
                @" 
                SELECT euser.[UserId], ra.[RoleId], euser.[FirstName], euser.[LastName], acc.[Username], euser.[Email], euser.[MobileNum], dept.[DepartmentName], meuser.[FirstName] AS ManagerFirstName, meuser.[LastName] AS ManagerLastName, meuser.[Email] AS ManagerEmail
                FROM [dbo].[EndUser] euser WITH(NOLOCK)
                INNER JOIN [dbo].[Account] acc WITH(NOLOCK) ON euser.[UserId] = acc.[UserId]
                INNER JOIN [dbo].[RoleAssignment] ra WITH(NOLOCK) ON euser.[UserId] = ra.[UserId]
                INNER JOIN [dbo].[Department] dept WITH(NOLOCK) ON euser.[DepartmentId] = dept.[DepartmentId]
                LEFT JOIN [dbo].[EndUser] meuser WITH(NOLOCK) ON euser.[ManagerId] = meuser.[UserId]
                WHERE acc.[Username] = @Username;";

            using (SqlDataReader reader = await DbCommand.GetDataWithConditionsAsync(GetUserByUsernameQuery, parameters))
            {
                if (reader.Read())
                {
                    user = new UserViewModel()
                    {
                        UserId = reader.GetInt16(reader.GetOrdinal("UserId")),
                        Username = reader.GetString(reader.GetOrdinal("Username")),
                        Role = new RoleModel()
                        {
                            RoleId = reader.GetInt16(reader.GetOrdinal("RoleId")),
                            RoleName = (RoleEnum)reader.GetInt16(reader.GetOrdinal("RoleId"))
                        },
                        FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                        LastName = reader.GetString(reader.GetOrdinal("LastName")),
                        Email = reader.GetString(reader.GetOrdinal("Email")),
                        MobileNum = reader.GetString(reader.GetOrdinal("MobileNum")),
                        Department = reader.GetString(reader.GetOrdinal("DepartmentName")),
                        ManagerFirstName = reader.IsDBNull(reader.GetOrdinal("ManagerFirstName")) ? null : reader.GetString(reader.GetOrdinal("ManagerFirstName")),
                        ManagerLastName = reader.IsDBNull(reader.GetOrdinal("ManagerLastName")) ? null : reader.GetString(reader.GetOrdinal("ManagerLastName")),
                        ManagerEmail = reader.IsDBNull(reader.GetOrdinal("ManagerEmail")) ? null : reader.GetString(reader.GetOrdinal("ManagerEmail"))
                    };
                }
            }
            return user;
        }

        // Getting a user with a different UserId, who has a specific username
        public async Task<AccountModel> GetByUsernameAndUserIdAsync(string username, int userId)
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
                WHERE acc.[Username] = @Username AND euser.[UserId] != @UserId;";

            using (SqlDataReader reader = await DbCommand.GetDataWithConditionsAsync(GetUserByUsernameAndUserId, parameters))
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

        public async Task<bool> UpdateAsync(AccountModel account)
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

            return await DbCommand.InsertUpdateDataAsync(UpdateAccountQuery, parameters) > 0;
        }
    }
}
