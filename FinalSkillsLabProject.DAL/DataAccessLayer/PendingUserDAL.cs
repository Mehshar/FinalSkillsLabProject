using System;
using System.Collections.Generic;
using FinalSkillsLabProject.DAL.Common;
using FinalSkillsLabProject.Common.Models;
using FinalSkillsLabProject.DAL.Interfaces;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace FinalSkillsLabProject.DAL.DataAccessLayer
{
    public class PendingUserDAL : IPendingUserDAL
    {
        private const string _AddPendingUserQuery =
            @"BEGIN TRANSACTION;

            DECLARE @salt UNIQUEIDENTIFIER=NEWID()            
            DECLARE @key int

            INSERT INTO [dbo].[PendingUser] ([NIC], [FirstName], [LastName], [Email], [MobileNum], [Username], [Password], [Salt])
            SELECT @NIC, @FirstName, @LastName, @Email, @MobileNum, @Username, HASHBYTES('SHA2_512', @Password+CAST(@salt AS NVARCHAR(36))), @salt;

            SELECT @key = @@IDENTITY

            COMMIT;";

        private const string _GetAllPendingUsersQuery =
            @"BEGIN TRANSACTION;

            SELECT *
            FROM [dbo].[PendingUser];

            COMMIT;";

        private const string _GetPendingUserByIdQuery =
            @"BEGIN TRANSACTION;

            SELECT *
            FROM [dbo].[PendingUser]
            WHERE [UserId] = @UserId;

            COMMIT;";

        public bool Add(PendingUserModel model)
        {
            List<SqlParameter> parameters = new List<SqlParameter>()
            {
                new SqlParameter("@NIC", model.NIC),
                new SqlParameter("@FirstName", model.FirstName),
                new SqlParameter("@LastName", model.LastName),
                new SqlParameter("@Email", model.Email),
                new SqlParameter("@MobileNum", model.MobileNum),
                new SqlParameter("@Username", model.Username),
                new SqlParameter("@Password", model.Password),
            };

            return DbCommand.InsertUpdateData(_AddPendingUserQuery, parameters) > 0;
        }

        public PendingUserModel GetById(int id)
        {
            PendingUserModel pendingUser = null;
            List<SqlParameter> parameters = new List<SqlParameter>()
            {
                new SqlParameter("@UserId", id)
            };

            DataTable dt = DbCommand.GetDataWithConditions(_GetPendingUserByIdQuery, parameters);

            if (dt.Rows.Count > 0)
            {
                DataRow row = dt.Rows[0];

                byte[] passwordBytes = (byte[])row["Password"];

                pendingUser = new PendingUserModel()
                {
                    UserId = int.Parse(row["UserId"].ToString()),
                    NIC = row["NIC"].ToString(),
                    FirstName = row["FirstName"].ToString(),
                    LastName = row["LastName"].ToString(),
                    Email = row["Email"].ToString(),
                    MobileNum = row["MobileNum"].ToString(),
                    Username = row["Username"].ToString(),
                    Password = BitConverter.ToString(passwordBytes).Replace("-", "")
                };
            }
            return pendingUser;
        }

        public IEnumerable<PendingUserModel> GetAll()
        {
            PendingUserModel pendingUser;
            List<PendingUserModel> pendingUsersList = new List<PendingUserModel>();

            DataTable dt = DbCommand.GetData(_GetAllPendingUsersQuery);

            foreach (DataRow row in dt.Rows)
            {
                pendingUser = new PendingUserModel()
                {
                    NIC = row["NIC"].ToString(),
                    FirstName = row["FirstName"].ToString(),
                    LastName = row["LastName"].ToString(),
                    Email = row["Email"].ToString(),
                    MobileNum = row["MobileNum"].ToString(),
                    Username = row["Username"].ToString()
                };

                pendingUsersList.Add(pendingUser);
            }
            return pendingUsersList;
        }
    }
}
