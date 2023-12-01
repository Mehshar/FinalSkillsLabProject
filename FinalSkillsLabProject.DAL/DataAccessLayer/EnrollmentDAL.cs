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

namespace FinalSkillsLabProject.DAL.DataAccessLayer
{
    public class EnrollmentDAL : IEnrollmentDAL
    {
        private const string _InsertEnrollmentQuery =
            @"BEGIN TRANSACTION;

            INSERT INTO [dbo].[Enrollment] ([UserId], [TrainingId], [EnrollmentDate], [EnrollmentStatus], [PrerequisiteMaterial])
            SELECT @UserId, @TrainingId, @EnrollmentDate, @EnrollmentStatus, @PrerequisiteMaterial;

            COMMIT;";

        private const string _UpdateEnrollmentQuery =
            @"BEGIN TRANSACTION;

            UPDATE [dbo].[Enrollment]
            SET [EnrollmentStatus] = @EnrollmentStatus,
	            [PrerequisiteMaterial] = @PrerequisiteMaterial,
                [DeclineReason] = @DeclineReason
            WHERE [UserId] = @UserId AND [TrainingId] = @TrainingId;

            COMMIT;";

        private const string _GetEnrollmentQuery =
            @"BEGIN TRANSACTION;

            SELECT *
            FROM [dbo].[Enrollment]
            WHERE [UserId] = @UserId AND [TrainingId] = @TrainingId;

            COMMIT;";

        private const string _GetAllEnrollmentsQuery =
            @"BEGIN TRANSACTION;

            SELECT *
            FROM [dbo].[Enrollment];

            COMMIT;";

        private const string _GetEnrollmentsByManager =
            @"BEGIN TRANSACTION;

            SELECT en.*
            FROM [dbo].[Enrollment] AS en
            INNER JOIN [dbo].[EndUser] AS euser
            ON en.[UserId] = euser.[UserId]
            WHERE euser.[Manager] = @Manager;

            COMMIT;";

        public bool Add(EnrollmentModel enrollment)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();

            parameters.Add(new SqlParameter("@UserId", enrollment.UserId));
            parameters.Add(new SqlParameter("@TrainingId", enrollment.TrainingId));
            parameters.Add(new SqlParameter("EnrollmentDate", enrollment.EnrollmentDate));
            parameters.Add(new SqlParameter("@EnrollmentStatus", enrollment.EnrollmentStatus));
            parameters.Add(new SqlParameter("@PrerequisiteMaterial", enrollment.PrerequisiteMaterial));

            return DbCommand.InsertUpdateData(_InsertEnrollmentQuery, parameters) > 0;
        }
        public bool Update(EnrollmentModel enrollment)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();

            parameters.Add(new SqlParameter("@EnrollmentStatus", enrollment.EnrollmentStatus));
            parameters.Add(new SqlParameter("@PrerequisiteMaterial", enrollment.PrerequisiteMaterial));
            parameters.Add(new SqlParameter("@UserId", enrollment.UserId));
            parameters.Add(new SqlParameter("@TrainingId", enrollment.TrainingId));
            parameters.Add(new SqlParameter("@DeclineReason", enrollment.DeclineReason));

            return DbCommand.InsertUpdateData(_UpdateEnrollmentQuery, parameters) > 0;
        }

        //public bool Delete(int userId, int trainingId) { }

        public EnrollmentModel Get(int userId, int trainingId)
        {
            List<SqlParameter> parameters = new List<SqlParameter>()
            { new SqlParameter("@UserId", userId) , new SqlParameter("@TrainingId", trainingId) };

            DataTable dt = DbCommand.GetDataWithConditions(_GetEnrollmentQuery, parameters);

            DataRow row = dt.Rows[0];

            EnrollmentModel enrollment = new EnrollmentModel()
            {
                UserId = userId,
                TrainingId = trainingId,
                EnrollmentDate = (DateTime)row["EnrollmentDate"],
                EnrollmentStatus = row["EnrollmentStatus"].ToString(),
                PrerequisiteMaterial = row["PrerequisiteMaterial"].ToString(),
                DeclineReason = row["DeclineReason"].ToString()
            };

            return enrollment;
        }
        public IEnumerable<EnrollmentModel> GetAll()
        {
            EnrollmentModel enrollment;
            List<EnrollmentModel> enrollmentsList = new List<EnrollmentModel>();

            DataTable dt = DbCommand.GetData(_GetAllEnrollmentsQuery);

            foreach (DataRow row in dt.Rows)
            {
                enrollment = new EnrollmentModel()
                {
                    UserId = int.Parse(row["UserId"].ToString()),
                    TrainingId = int.Parse(row["TrainingId"].ToString()),
                    EnrollmentDate = (DateTime)row["EnrollmentDate"],
                    EnrollmentStatus = row["EnrollmentStatus"].ToString(),
                    PrerequisiteMaterial = row["PrerequisiteMaterial"].ToString(),
                    DeclineReason = row["DeclineReason"].ToString()
                };

                enrollmentsList.Add(enrollment);
            }
            return enrollmentsList;
        }

        public IEnumerable<EnrollmentModel> GetAllByManager(string manager)
        {
            EnrollmentModel enrollment;
            List<EnrollmentModel> enrollmentsList = new List<EnrollmentModel>();

            List<SqlParameter> parameters = new List<SqlParameter>() { new SqlParameter("@Manager", manager) };

            DataTable dt = DbCommand.GetDataWithConditions(_GetEnrollmentsByManager, parameters);

            foreach (DataRow row in dt.Rows)
            {
                enrollment = new EnrollmentModel()
                {
                    UserId = int.Parse(row["UserId"].ToString()),
                    TrainingId = int.Parse(row["TrainingId"].ToString()),
                    EnrollmentDate = (DateTime)row["EnrollmentDate"],
                    EnrollmentStatus = row["EnrollmentStatus"].ToString(),
                    PrerequisiteMaterial = row["PrerequisiteMaterial"].ToString()
                };
                enrollmentsList.Add(enrollment);
            }
            return enrollmentsList;
        }
    }
}
