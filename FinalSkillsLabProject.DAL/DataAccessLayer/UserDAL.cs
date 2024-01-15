using FinalSkillsLabProject.Common.Enums;
using System;
using System.Collections.Generic;
using FinalSkillsLabProject.DAL.Common;
using FinalSkillsLabProject.Common.Models;
using FinalSkillsLabProject.DAL.Interfaces;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace FinalSkillsLabProject.DAL.DataAccessLayer
{
    public class UserDAL : IUserDAL
    {
        //public async Task<bool> AddAsync(SignUpModel model)
        //{
        //    List<SqlParameter> parameters = new List<SqlParameter>()
        //    {
        //        new SqlParameter("@NIC", model.NIC),
        //        new SqlParameter("@FirstName", model.FirstName),
        //        new SqlParameter("@LastName", model.LastName),
        //        new SqlParameter("@Email", model.Email),
        //        new SqlParameter("@MobileNum", model.MobileNum),
        //        new SqlParameter("@ManagerId", Convert.ToInt16(model.ManagerId)),
        //        new SqlParameter("@DepartmentId", Convert.ToInt16(model.DepartmentId)),
        //        new SqlParameter("@RoleId", Convert.ToInt16((int)RoleEnum.Employee)),
        //        new SqlParameter("@Username", model.Username),
        //        new SqlParameter("@Password", model.Password)
        //    };

        //    const string AddUserQuery =
        //        @"BEGIN TRANSACTION;

        //        DECLARE @key int
        //        DECLARE @salt UNIQUEIDENTIFIER=NEWID()

        //        INSERT INTO [dbo].[EndUser] ([NIC], [FirstName], [LastName], [Email], [MobileNum], [ManagerId], [DepartmentId])
        //        SELECT @NIC, @FirstName, @LastName, @Email, @MobileNum, @ManagerId, @DepartmentId;

        //        SELECT @key = @@IDENTITY

        //        INSERT INTO [dbo].[RoleAssignment] ([UserId], [RoleId])
        //        SELECT @key, @RoleId;

        //        INSERT INTO [dbo].[Account] ([Username], [UserId], [Password], [Salt])
        //        SELECT @Username, @key, HASHBYTES('SHA2_512', @Password+CAST(@salt AS NVARCHAR(36))), @salt;

        //        COMMIT;";

        //    return await DbCommand.InsertUpdateDataAsync(AddUserQuery, parameters) > 0;
        //}

        public async Task<bool> AddAsync(SignUpModel model)
        {
            const string AddUserQuery =
                @"BEGIN TRANSACTION;

                DECLARE @key int

                INSERT INTO [dbo].[EndUser] ([NIC], [FirstName], [LastName], [Email], [MobileNum], [ManagerId], [DepartmentId])
                SELECT @NIC, @FirstName, @LastName, @Email, @MobileNum, @ManagerId, @DepartmentId;

                SELECT @key = @@IDENTITY

                INSERT INTO [dbo].[RoleAssignment] ([UserId], [RoleId])
                SELECT @key, @RoleId;

                INSERT INTO [dbo].[Account] ([Username], [UserId], [Password], [Salt])
                SELECT @Username, @key, @Password, @Salt;

                COMMIT;";

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
                new SqlParameter("@Password", model.HashedPassword),
                new SqlParameter("@Salt", model.Salt)
            };

            return await DbCommand.InsertUpdateDataAsync(AddUserQuery, parameters) > 0;
        }

        public async Task<bool> UpdateAsync(UserModel user)
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
	                [ManagerId] = @ManagerId
                WHERE [UserId] = @UserId;";

            return await DbCommand.InsertUpdateDataAsync(UpdateUserQuery, parameters) > 0;
        }

        public async Task<bool> DeleteAsync(int userId)
        {
            SqlParameter parameter = new SqlParameter("@UserId", userId);

            const string DeleteUserQuery =
              @"DELETE FROM [dbo].[EndUser]
                WHERE [UserId] = @UserId;";

            return await DbCommand.DeleteDataAsync(DeleteUserQuery, parameter) > 0;
        }

        public async Task<UserModel> GetAsync(int userId)
        {
            UserModel user = null;
            List<SqlParameter> parameters = new List<SqlParameter>()
            {
                new SqlParameter("@UserId", userId)
            };

            const string GetUserQuery =
              @"SELECT eu.*, ra.[RoleId]
                FROM [dbo].[EndUser] eu
                INNER JOIN [dbo].[RoleAssignment] ra
                ON eu.[UserId] = ra.[UserId]
                WHERE eu.[UserId] = @UserId;";

            using (SqlDataReader reader = await DbCommand.GetDataWithConditionsAsync(GetUserQuery, parameters))
            {
                if (reader.Read())
                {
                    user = new UserModel()
                    {
                        UserId = userId,
                        NIC = reader.GetString(reader.GetOrdinal("NIC")),
                        FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                        LastName = reader.GetString(reader.GetOrdinal("LastName")),
                        Email = reader.GetString(reader.GetOrdinal("Email")),
                        MobileNum = reader.GetString(reader.GetOrdinal("MobileNum")),
                        DepartmentId = reader.GetInt16(reader.GetOrdinal("DepartmentId")),
                        ManagerId = reader.IsDBNull(reader.GetOrdinal("ManagerId")) ? null : (int?)reader.GetInt16(reader.GetOrdinal("ManagerId")),
                        RoleId = reader.GetInt16(reader.GetOrdinal("RoleId"))
                    };
                }
            }
            return user;
        }

        public async Task<IEnumerable<UserModel>> GetAllAsync()
        {
            UserModel user;
            List<UserModel> usersList = new List<UserModel>();

            const string GetAllUsersQuery =
              @"SELECT eu.*, ra.[RoleId]
                FROM [dbo].[EndUser] eu
                INNER JOIN [dbo].[RoleAssignment] ra
                ON eu.[UserId] = ra.[UserId];";

            using (SqlDataReader reader = await DbCommand.GetDataAsync(GetAllUsersQuery))
            {
                while (reader.Read())
                {
                    user = new UserModel()
                    {
                        UserId = reader.GetInt16(reader.GetOrdinal("UserId")),
                        NIC = reader.GetString(reader.GetOrdinal("NIC")),
                        FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                        LastName = reader.GetString(reader.GetOrdinal("LastName")),
                        Email = reader.GetString(reader.GetOrdinal("Email")),
                        MobileNum = reader.GetString(reader.GetOrdinal("MobileNum")),
                        DepartmentId = reader.GetInt16(reader.GetOrdinal("DepartmentId")),
                        ManagerId = reader.IsDBNull(reader.GetOrdinal("ManagerId")) ? null : (int?)reader.GetInt16(reader.GetOrdinal("ManagerId")),
                        RoleId = reader.GetInt16(reader.GetOrdinal("RoleId"))
                    };
                    usersList.Add(user);
                }
            }
            return usersList;
        }
    }
}
