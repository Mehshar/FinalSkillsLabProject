using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FinalSkillsLabProject.Common.Models;
using FinalSkillsLabProject.DAL.Common;
using FinalSkillsLabProject.DAL.Interfaces;
using FinalSkillsLabProject.Common.Enums;

namespace FinalSkillsLabProject.DAL.DataAccessLayer
{
    public class EnrollmentDAL : IEnrollmentDAL
    {
        public bool Add(EnrollmentModel enrollment, List<PrerequisiteMaterialModel> prerequisiteMaterialsList)
        {
            List<SqlParameter> parameters = new List<SqlParameter>()
            {
                new SqlParameter("@UserId", enrollment.UserId),
                new SqlParameter("@TrainingId", enrollment.TrainingId),
                new SqlParameter("@EnrollmentDate", DateTime.Now),
                new SqlParameter("@EnrollmentStatus", EnrollmentStatusEnum.Pending.ToString()),
                new SqlParameter("@DeclineReason", enrollment.DeclineReason)
            };

            SqlParameter outputParameter = new SqlParameter("@EnrollmentId", SqlDbType.Int);
            outputParameter.Direction = ParameterDirection.Output;
            parameters.Add(outputParameter);

            const string InsertEnrollmentQuery =
                @"BEGIN TRANSACTION;

                INSERT INTO [dbo].[Enrollment] ([UserId], [TrainingId], [EnrollmentDate], [EnrollmentStatus], [DeclineReason])
                VALUES (@UserId, @TrainingId, @EnrollmentDate, @EnrollmentStatus, @DeclineReason);

                SET @EnrollmentId = SCOPE_IDENTITY();

                COMMIT;";

            int rowsAffected = DbCommand.InsertUpdateData(InsertEnrollmentQuery, parameters);

            if (rowsAffected > 0)
            {
                // Get the EnrollmentId from the output parameter
                int enrollmentId = Convert.ToInt32(outputParameter.Value);

                foreach (var prerequisiteMaterial in prerequisiteMaterialsList)
                {
                    List<SqlParameter> prerequisiteMaterialParameters = new List<SqlParameter>()
                    {
                        new SqlParameter("@EnrollmentId", enrollmentId),
                        new SqlParameter("@PrerequisiteId", prerequisiteMaterial.PrerequisiteId),
                        new SqlParameter("@URL", prerequisiteMaterial.PrerequisiteMaterialURL)
                    };

                    const string InsertPrerequisiteMaterialQuery =
                        @"
                        INSERT INTO [dbo].[PrerequisiteMaterial] ([EnrollmentId], [PrerequisiteId], [URL])
                        VALUES (@EnrollmentId, @PrerequisiteId, @URL)";

                    DbCommand.InsertUpdateData(InsertPrerequisiteMaterialQuery, prerequisiteMaterialParameters);
                }
                return true;
            }
            return false;
        }

        public bool Update(EnrollmentModel enrollment)
        {
            //List<SqlParameter> parameters = new List<SqlParameter>();

            //parameters.Add(new SqlParameter("@EnrollmentStatus", enrollment.EnrollmentStatus));
            //parameters.Add(new SqlParameter("@PrerequisiteMaterial", enrollment.PrerequisiteMaterial));
            //parameters.Add(new SqlParameter("@UserId", enrollment.UserId));
            //parameters.Add(new SqlParameter("@TrainingId", enrollment.TrainingId));
            //parameters.Add(new SqlParameter("@DeclineReason", enrollment.DeclineReason));

            //const string UpdateEnrollmentQuery =
            //  @"UPDATE [dbo].[Enrollment]
            //    SET [EnrollmentStatus] = @EnrollmentStatus,
            //     [PrerequisiteMaterial] = @PrerequisiteMaterial,
            //        [DeclineReason] = @DeclineReason
            //    WHERE [UserId] = @UserId AND [TrainingId] = @TrainingId;";

            //return DbCommand.InsertUpdateData(UpdateEnrollmentQuery, parameters) > 0;
            throw new NotImplementedException();
        }

        //public bool Delete(int userId, int trainingId) { }

        public EnrollmentModel Get(int userId, int trainingId)
        {
            EnrollmentModel enrollment = null;
            List<SqlParameter> parameters = new List<SqlParameter>()
            { new SqlParameter("@UserId", userId) , new SqlParameter("@TrainingId", trainingId) };

            const string GetEnrollmentQuery =
              @"SELECT *
                FROM [dbo].[Enrollment]
                WHERE [UserId] = @UserId AND [TrainingId] = @TrainingId;";

            using (SqlDataReader reader = DbCommand.GetDataWithConditions(GetEnrollmentQuery, parameters))
            {
                if (reader.Read())
                {
                    enrollment = new EnrollmentModel()
                    {
                        EnrollmentId = reader.GetInt16(reader.GetOrdinal("EnrollmentId")),
                        UserId = userId,
                        TrainingId = trainingId,
                        EnrollmentDate = reader.GetDateTime(reader.GetOrdinal("EnrollmentDate")),
                        EnrollmentStatus = reader.GetString(reader.GetOrdinal("EnrollmentStatus")),
                        DeclineReason = reader.GetString(reader.GetOrdinal("DeclineReason")),
                    };
                }
            }
            return enrollment;
        }

        public IEnumerable<EnrollmentModel> GetAll()
        {
            EnrollmentModel enrollment;
            List<EnrollmentModel> enrollmentsList = new List<EnrollmentModel>();

            const string GetAllEnrollmentsQuery =
              @"SELECT *
                FROM [dbo].[Enrollment];";

            using (SqlDataReader reader = DbCommand.GetData(GetAllEnrollmentsQuery))
            {
                while (reader.Read())
                {
                    enrollment = new EnrollmentModel()
                    {
                        EnrollmentId = reader.GetInt16(reader.GetOrdinal("EnrollmentId")),
                        UserId = reader.GetInt16(reader.GetOrdinal("UserId")),
                        TrainingId = reader.GetInt16(reader.GetOrdinal("TrainingId")),
                        EnrollmentDate = reader.GetDateTime(reader.GetOrdinal("EnrollmentDate")),
                        EnrollmentStatus = reader.GetString(reader.GetOrdinal("EnrollmentStatus")),
                        DeclineReason = reader.GetString(reader.GetOrdinal("DeclineReason"))
                    };
                    enrollmentsList.Add(enrollment);
                }
            }
            return enrollmentsList;
        }

        public IEnumerable<EnrollmentModel> GetAllByManager(string manager)
        {
            EnrollmentModel enrollment;
            List<EnrollmentModel> enrollmentsList = new List<EnrollmentModel>();

            List<SqlParameter> parameters = new List<SqlParameter>() { new SqlParameter("@Manager", manager) };

            const string GetEnrollmentsByManager =
              @"SELECT en.*
                FROM [dbo].[Enrollment] AS en
                INNER JOIN [dbo].[EndUser] AS euser
                ON en.[UserId] = euser.[UserId]
                WHERE euser.[Manager] = @Manager;";

            using (SqlDataReader reader = DbCommand.GetDataWithConditions(GetEnrollmentsByManager, parameters))
            {
                while (reader.Read())
                {
                    enrollment = new EnrollmentModel()
                    {
                        EnrollmentId = reader.GetInt16(reader.GetOrdinal("EnrollmentId")),
                        UserId = reader.GetInt16(reader.GetOrdinal("UserId")),
                        TrainingId = reader.GetInt16(reader.GetOrdinal("TrainingId")),
                        EnrollmentDate = reader.GetDateTime(reader.GetOrdinal("EnrollmentDate")),
                        EnrollmentStatus = reader.GetString(reader.GetOrdinal("EnrollmentStatus"))
                    };
                    enrollmentsList.Add(enrollment);
                }
            }
            return enrollmentsList;
        }
    }
}
