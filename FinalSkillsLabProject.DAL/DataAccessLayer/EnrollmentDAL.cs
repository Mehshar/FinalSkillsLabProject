using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using FinalSkillsLabProject.Common.Models;
using FinalSkillsLabProject.DAL.Common;
using FinalSkillsLabProject.DAL.Interfaces;
using FinalSkillsLabProject.Common.Enums;
using System.Threading.Tasks;

namespace FinalSkillsLabProject.DAL.DataAccessLayer
{
    public class EnrollmentDAL : IEnrollmentDAL
    {
        public async Task<bool> AddAsync(EnrollmentModel enrollment, List<PrerequisiteMaterialModel> prerequisiteMaterialsList)
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

            int rowsAffected = await DbCommand.InsertUpdateDataAsync(InsertEnrollmentQuery, parameters);

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

                    await DbCommand.InsertUpdateDataAsync(InsertPrerequisiteMaterialQuery, prerequisiteMaterialParameters);
                }
                return true;
            }
            return false;
        }

        public async Task<bool> UpdateAsync(int enrollmentId, bool isApproved, string declineReason)
        {
            List<SqlParameter> parameters;
            string UpdateQuery;

            if (isApproved)
            {
                UpdateQuery = @"UPDATE [dbo].[Enrollment]
                                SET [EnrollmentStatus] = @EnrollmentStatus,
	                                [DeclineReason] = DEFAULT
                                WHERE [EnrollmentId] = @EnrollmentId;";

                parameters = new List<SqlParameter>()
                {
                    new SqlParameter("@EnrollmentStatus", EnrollmentStatusEnum.Approved.ToString()),
                    new SqlParameter("@EnrollmentId", enrollmentId)
                };
            }
            else
            {
                UpdateQuery = @"UPDATE [dbo].[Enrollment]
                                SET [EnrollmentStatus] = @EnrollmentStatus,
	                                [DeclineReason] = @DeclineReason
                                WHERE [EnrollmentId] = @EnrollmentId;";

                parameters = new List<SqlParameter>()
                {
                    new SqlParameter("@EnrollmentStatus", EnrollmentStatusEnum.Declined.ToString()),
                    new SqlParameter("@DeclineReason", declineReason),
                    new SqlParameter("@EnrollmentId", enrollmentId)
                };
            }
            return await DbCommand.InsertUpdateDataAsync(UpdateQuery, parameters) > 0;
        }

        public EnrollmentModel Get(int userId, int trainingId)
        {
            EnrollmentModel enrollment = null;
            List<SqlParameter> parameters = new List<SqlParameter>()
            { new SqlParameter("@UserId", userId) , new SqlParameter("@TrainingId", trainingId) };

            const string GetEnrollmentQuery =
                @"
                SELECT *
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

        public IEnumerable<EnrollmentViewModel> GetAll()
        {
            EnrollmentViewModel enrollment;
            List<EnrollmentViewModel> enrollmentsList = new List<EnrollmentViewModel>();

            const string GetAllEnrollmentsQuery =
              @"SELECT euser.[UserId], euser.[FirstName] AS EFirstName, euser.[LastName] AS ELastName, euser.[Email] AS EEmail, d.[DepartmentName], meuser.[FirstName] AS MFirstName, meuser.[LastName] AS MLastName, meuser.[Email] AS MEmail, t.[TrainingName], pd.[DepartmentName] AS PriorityDeptName, e.[EnrollmentDate], e.[EnrollmentId], e.[EnrollmentStatus]
                FROM [dbo].[Enrollment] AS e
                INNER JOIN [dbo].[Training] AS t
                ON e.[TrainingId] = t.[TrainingId]
                INNER JOIN [dbo].[Department] AS pd
                ON t.[PriorityDepartment] = pd.[DepartmentId]
                INNER JOIN [dbo].[EndUser] AS euser
                ON e.[UserId] = euser.[UserId]
                INNER JOIN [dbo].[Department] AS d
                ON euser.[DepartmentId] = d.[DepartmentId]
                LEFT JOIN [dbo].[EndUser] AS meuser
                ON meuser.[UserId] = euser.[ManagerId];";

            using (SqlDataReader reader = DbCommand.GetData(GetAllEnrollmentsQuery))
            {
                while (reader.Read())
                {
                    enrollment = new EnrollmentViewModel()
                    {
                        EnrollmentId = reader.GetInt16(reader.GetOrdinal("EnrollmentId")),
                        EmployeeId = reader.GetInt16(reader.GetOrdinal("UserId")),
                        FirstName = reader.GetString(reader.GetOrdinal("EFirstName")),
                        LastName = reader.GetString(reader.GetOrdinal("ELastName")),
                        Email = reader.GetString(reader.GetOrdinal("EEmail")),
                        EmployeeDepartment = reader.GetString(reader.GetOrdinal("DepartmentName")),
                        EnrollmentDate = reader.GetDateTime(reader.GetOrdinal("EnrollmentDate")),
                        TrainingName = reader.GetString(reader.GetOrdinal("TrainingName")),
                        PriorityDepartmentName = reader.GetString(reader.GetOrdinal("PriorityDeptName")),
                        EnrollmentStatus = reader.GetString(reader.GetOrdinal("EnrollmentStatus")),
                        ManagerFirstName = reader.GetString(reader.GetOrdinal("MFirstName")),
                        ManagerLastName = reader.GetString(reader.GetOrdinal("MLastName")),
                        ManagerEmail = reader.GetString(reader.GetOrdinal("MEmail"))
                    };
                    enrollmentsList.Add(enrollment);
                }
            }
            return enrollmentsList;
        }

        public IEnumerable<EnrollmentViewModel> GetAllByManagerTraining(int managerId, int trainingId)
        {
            EnrollmentViewModel enrollment;
            List<EnrollmentViewModel> enrollmentsList = new List<EnrollmentViewModel>();

            List<SqlParameter> parameters = new List<SqlParameter>() { new SqlParameter("@ManagerId", managerId), new SqlParameter("@TrainingId", trainingId) };

            const string GetEnrollmentsByTrainingManager =
                @"
                SELECT en.[EnrollmentId], euser.[UserId], euser.[FirstName], euser.[LastName], euser.[Email], en.[EnrollmentDate], t.[TrainingId], t.[TrainingName], t.[PriorityDepartment], dept.[DepartmentName] AS [PriorityDepartmentName], t.[Capacity]
                FROM [dbo].[Enrollment] AS en
                INNER JOIN [dbo].[Training] AS t
                ON en.[TrainingId] = t.[TrainingId]
                INNER JOIN [dbo].[Department] as dept
                ON t.[PriorityDepartment] = dept.[DepartmentId]
                INNER JOIN [dbo].[EndUser] AS euser
                ON en.[UserId] = euser.[UserId]
                INNER JOIN [dbo].[Department] AS d
                ON euser.[DepartmentId] = d.[DepartmentId]
                WHERE euser.ManagerId = @ManagerId and t.TrainingId = @TrainingId;";

            using (SqlDataReader reader = DbCommand.GetDataWithConditions(GetEnrollmentsByTrainingManager, parameters))
            {
                while (reader.Read())
                {
                    enrollment = new EnrollmentViewModel()
                    {
                        EnrollmentId = reader.GetInt16(reader.GetOrdinal("EnrollmentId")),
                        EmployeeId = reader.GetInt16(reader.GetOrdinal("UserId")),
                        FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                        LastName = reader.GetString(reader.GetOrdinal("LastName")),
                        Email = reader.GetString(reader.GetOrdinal("Email")),
                        EnrollmentDate = reader.GetDateTime(reader.GetOrdinal("EnrollmentDate")),
                        TrainingName = reader.GetString(reader.GetOrdinal("TrainingName")),
                        TrainingId = trainingId,
                        PriorityDepartmentName = reader.GetString(reader.GetOrdinal("PriorityDepartmentName")),
                        Capacity = reader.GetInt16(reader.GetOrdinal("Capacity"))
                    };
                    enrollmentsList.Add(enrollment);
                }
            }
            return enrollmentsList;
        }

        public IEnumerable<EnrollmentViewModel> GetAllByManager(int managerId)
        {
            EnrollmentViewModel enrollment = null;
            List<EnrollmentViewModel> enrollmentsList = new List<EnrollmentViewModel>();

            const string GetEnrollmentsByManager =
                @"
                SELECT en.[EnrollmentId], en.[EnrollmentStatus], euser.[UserId], euser.[FirstName], euser.[LastName], euser.[Email], d.[DepartmentName], en.[EnrollmentDate], t.[TrainingId], t.[TrainingName], t.[PriorityDepartment], dept.[DepartmentName] AS [PriorityDepartmentName], t.[Capacity]
                FROM [dbo].[Enrollment] AS en
                INNER JOIN [dbo].[Training] AS t
                ON en.[TrainingId] = t.[TrainingId]
                INNER JOIN [dbo].[Department] as dept
                ON t.[PriorityDepartment] = dept.[DepartmentId]
                INNER JOIN [dbo].[EndUser] AS euser
                ON en.[UserId] = euser.[UserId]
                INNER JOIN [dbo].[Department] AS d
                ON euser.[DepartmentId] = d.[DepartmentId]
                WHERE euser.ManagerId = @ManagerId;";

            List<SqlParameter> parameters = new List<SqlParameter>() { new SqlParameter("@ManagerId", managerId) };

            using (SqlDataReader reader = DbCommand.GetDataWithConditions(GetEnrollmentsByManager, parameters))
            {
                while (reader.Read())
                {
                    enrollment = new EnrollmentViewModel()
                    {
                        EnrollmentId = reader.GetInt16(reader.GetOrdinal("EnrollmentId")),
                        EnrollmentStatus = reader.GetString(reader.GetOrdinal("EnrollmentStatus")),
                        EmployeeId = reader.GetInt16(reader.GetOrdinal("UserId")),
                        FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                        LastName = reader.GetString(reader.GetOrdinal("LastName")),
                        Email = reader.GetString(reader.GetOrdinal("Email")),
                        EmployeeDepartment = reader.GetString(reader.GetOrdinal("DepartmentName")),
                        EnrollmentDate = reader.GetDateTime(reader.GetOrdinal("EnrollmentDate")),
                        TrainingName = reader.GetString(reader.GetOrdinal("TrainingName")),
                        TrainingId = reader.GetInt16(reader.GetOrdinal("TrainingId")),
                        PriorityDepartmentName = reader.GetString(reader.GetOrdinal("PriorityDepartmentName")),
                        Capacity = reader.GetInt16(reader.GetOrdinal("Capacity"))
                    };
                    enrollmentsList.Add(enrollment);
                }
            }
            return enrollmentsList;
        }

        public IEnumerable<PrerequisiteMaterialViewModel> GetPrerequisiteMaterialsByEnrollment(int enrollmentId)
        {
            PrerequisiteMaterialViewModel prerequisiteMaterial = null;
            List<PrerequisiteMaterialViewModel> prerequisiteMaterialsList = new List<PrerequisiteMaterialViewModel>();

            const string GetPrerequisiteMaterialsByEnrollment =
                @"SELECT pm.*, p.[Description], e.[EnrollmentStatus]
                FROM [dbo].[PrerequisiteMaterial] AS pm
                INNER JOIN [dbo].[Prerequisite] AS p
                ON pm.[PrerequisiteId] = p.[PrerequisiteId]
                INNER JOIN [dbo].[Enrollment] AS e
                ON pm.[EnrollmentId] = e.EnrollmentId
                WHERE pm.[EnrollmentId] = @EnrollmentId";

            List<SqlParameter> parameters = new List<SqlParameter>() { new SqlParameter("@EnrollmentId", enrollmentId) };

            using (SqlDataReader reader = DbCommand.GetDataWithConditions(GetPrerequisiteMaterialsByEnrollment, parameters))
            {
                while (reader.Read())
                {
                    prerequisiteMaterial = new PrerequisiteMaterialViewModel()
                    {
                        PrerequisiteMaterialId = reader.GetInt16(reader.GetOrdinal("PrerequisiteMaterialId")),
                        EnrollmentId = reader.GetInt16(reader.GetOrdinal("EnrollmentId")),
                        PrerequisiteId = reader.GetInt16(reader.GetOrdinal("PrerequisiteId")),
                        Description = reader.GetString(reader.GetOrdinal("Description")),
                        PrerequisiteMaterialURL = reader.GetString(reader.GetOrdinal("URL")),
                        EnrollmentStatus = reader.GetString(reader.GetOrdinal("EnrollmentStatus"))
                    };
                    prerequisiteMaterialsList.Add(prerequisiteMaterial);
                }
            }
            return prerequisiteMaterialsList;
        }

        public UserEnrollmentViewModel GetUserByEnrollment(int enrollmentId)
        {
            const string GetUserByEnrollment =
                @"SELECT euser.[Email], a.[Username], t.[TrainingName], meuser.[FirstName] AS MFirstName, meuser.[LastName] AS MLastName, meuser.[Email] AS MEmail
                FROM [dbo].[Enrollment] AS e
                INNER JOIN [dbo].[Training] AS t
                ON e.[TrainingId] = t.[TrainingId]
                INNER JOIN [dbo].[EndUser] AS euser
                ON e.[UserId] = euser.[UserId]
                LEFT JOIN [dbo].[EndUser] AS meuser
                ON meuser.[UserId] = euser.[ManagerId]
                INNER JOIN [dbo].[Account] AS a
                ON e.[UserId] = a.[UserId]
                WHERE e.[EnrollmentId] = @EnrollmentId;";

            UserEnrollmentViewModel user = null;
            List<SqlParameter> parameters = new List<SqlParameter>() { new SqlParameter("@EnrollmentId", enrollmentId) };

            using (SqlDataReader reader = DbCommand.GetDataWithConditions(GetUserByEnrollment, parameters))
            {
                while (reader.Read())
                {
                    user = new UserEnrollmentViewModel()
                    {
                        Email = reader.GetString(reader.GetOrdinal("Email")),
                        Username = reader.GetString(reader.GetOrdinal("Username")),
                        TrainingName = reader.GetString(reader.GetOrdinal("TrainingName")),
                        ManagerFirstName = reader.GetString(reader.GetOrdinal("MFirstName")),
                        ManagerLastName = reader.GetString(reader.GetOrdinal("MLastName")),
                        ManagerEmail = reader.GetString(reader.GetOrdinal("MEmail"))
                    };
                }
            }
            return user;
        }

        public string GetDeclineReasonByEnrollment(int enrollmentId)
        {
            string declineReason = null;

            const string GetDeclineReasonByEnrollment =
                @"SELECT [DeclineReason]
                FROM [dbo].[Enrollment]
                WHERE [EnrollmentId] = @EnrollmentId";

            List<SqlParameter> parameters = new List<SqlParameter>() { new SqlParameter("@EnrollmentId", enrollmentId) };

            using (SqlDataReader reader = DbCommand.GetDataWithConditions(GetDeclineReasonByEnrollment, parameters))
            {
                if (reader.Read())
                {
                    declineReason = reader.GetString(reader.GetOrdinal("DeclineReason"));
                }
            }
            return declineReason;
        }
    }
}
