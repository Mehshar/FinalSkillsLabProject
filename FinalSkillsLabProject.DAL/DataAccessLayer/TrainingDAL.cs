using System;
using System.Collections.Generic;
using FinalSkillsLabProject.DAL.Common;
using FinalSkillsLabProject.Common.Models;
using FinalSkillsLabProject.DAL.Interfaces;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace FinalSkillsLabProject.DAL.DataAccessLayer
{
    public class TrainingDAL : ITrainingDAL
    {
        public async Task AddAsync(TrainingModel training, List<int> prerequisitesList)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();

            parameters.Add(new SqlParameter("@TrainingName", training.TrainingName));
            parameters.Add(new SqlParameter("@Description", training.Description));
            parameters.Add(new SqlParameter("@Deadline", training.Deadline));
            if (training.PriorityDepartment == -1)
            {
                parameters.Add(new SqlParameter("@PriorityDepartment", DBNull.Value));
            }
            else
            {
                parameters.Add(new SqlParameter("@PriorityDepartment", training.PriorityDepartment));
            }

            parameters.Add(new SqlParameter("@Capacity", training.Capacity));

            if (prerequisitesList == null)
            {
                parameters.Add(new SqlParameter("@PrerequisiteIds", ""));
            }
            else
            {
                parameters.Add(new SqlParameter("@PrerequisiteIds", string.Join(",", prerequisitesList)));
            }

            const string InsertTrainingQuery =
                @"BEGIN TRANSACTION;

                DECLARE @training_key INT            

                INSERT INTO [dbo].[Training] ([TrainingName], [Description], [Deadline], [PriorityDepartment], [Capacity])
                SELECT @TrainingName, @Description, @Deadline, @PriorityDepartment, @Capacity;

                SELECT @training_key = @@IDENTITY

                -- Check if PrerequisiteIds is not empty
                IF LEN(@PrerequisiteIds) > 0
                BEGIN
	                -- Loop through the list of prerequisite IDs and insert each one
	                DECLARE @index INT = 1;

	                WHILE @index <= LEN(@PrerequisiteIds)
	                BEGIN
	                    DECLARE @PrerequisiteId INT;
	                    SELECT @PrerequisiteId = CAST(SUBSTRING(@PrerequisiteIds, @index, CHARINDEX(',', @PrerequisiteIds + ',', @index) - @index) AS INT);

	                    INSERT INTO [dbo].[Training_Prerequisite] ([TrainingId], [PrerequisiteId])
	                    SELECT @training_key, @PrerequisiteId;

	                    SET @index = CHARINDEX(',', @PrerequisiteIds + ',', @index) + 1;
	                END;
                END;

                COMMIT";

            await DbCommand.InsertUpdateDataAsync(InsertTrainingQuery, parameters);
        }

        public async Task UpdateAsync(TrainingModel training)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();

            parameters.Add(new SqlParameter("@TrainingName", training.TrainingName));
            parameters.Add(new SqlParameter("@Description", training.Description));
            parameters.Add(new SqlParameter("@Deadline", training.Deadline));
            if (training.PriorityDepartment == -1)
            {
                parameters.Add(new SqlParameter("@PriorityDepartment", DBNull.Value));
            }
            else
            {
                parameters.Add(new SqlParameter("@PriorityDepartment", Convert.ToInt16(training.PriorityDepartment)));
            }
            parameters.Add(new SqlParameter("@Capacity", Convert.ToInt16(training.Capacity)));
            parameters.Add(new SqlParameter("@TrainingId", Convert.ToInt16(training.TrainingId)));

            const string UpdateTrainingQuery =
              @"UPDATE [dbo].[Training]
                SET [TrainingName] = @TrainingName,
	                [Description] = @Description,
	                [Deadline] = @Deadline,
	                [PriorityDepartment] = @PriorityDepartment,
	                [Capacity] = @Capacity
                WHERE [TrainingId] = @TrainingId;";

            await DbCommand.InsertUpdateDataAsync(UpdateTrainingQuery, parameters);
        }

        public async Task<bool> DeleteAsync(int trainingId)
        {
            SqlParameter parameter = new SqlParameter("@TrainingId", trainingId);

            const string DeleteTrainingQuery =
              @"DELETE FROM [dbo].[Training]
                WHERE [TrainingId] = @TrainingId;";

            return await DbCommand.DeleteDataAsync(DeleteTrainingQuery, parameter) > 0;
        }

        public async Task<TrainingModel> GetAsync(int trainingId)
        {
            TrainingModel training = null;
            List<SqlParameter> parameters = new List<SqlParameter>();

            parameters.Add(new SqlParameter("@TrainingId", trainingId));

            const string GetTrainingQuery =
              @"SELECT *
                FROM [dbo].[Training] AS t
                LEFT JOIN [dbo].[Department] AS d
                ON t.[PriorityDepartment] = d.[DepartmentId]
                WHERE [TrainingId] = @TrainingId;";

            using (SqlDataReader reader = await DbCommand.GetDataWithConditionsAsync(GetTrainingQuery, parameters))
            {
                if (reader.Read())
                {
                    training = new TrainingModel()
                    {
                        TrainingId = trainingId,
                        TrainingName = reader.GetString(reader.GetOrdinal("TrainingName")),
                        Description = reader.GetString(reader.GetOrdinal("Description")),
                        Deadline = reader.GetDateTime(reader.GetOrdinal("Deadline")),
                        PriorityDepartment = reader.IsDBNull(reader.GetOrdinal("PriorityDepartment")) ? null : (int?)reader.GetInt16(reader.GetOrdinal("PriorityDepartment")),
                        PriorityDepartmentName = reader.IsDBNull(reader.GetOrdinal("DepartmentName")) ? null : reader.GetString(reader.GetOrdinal("DepartmentName")),
                        Capacity = reader.GetInt16(reader.GetOrdinal("Capacity"))
                    };
                }
            }
            return training;
        }

        public async Task<IEnumerable<TrainingModel>> GetAllAsync()
        {
            TrainingModel training;
            List<TrainingModel> trainingList = new List<TrainingModel>();

            const string GetAllTrainingsQuery =
              @"SELECT *
                FROM [dbo].[Training] as t
                LEFT JOIN [dbo].[Department] as d
                ON t.[PriorityDepartment] = d.DepartmentId;";

            using (SqlDataReader reader = await DbCommand.GetDataAsync(GetAllTrainingsQuery))
            {
                while (reader.Read())
                {
                    training = new TrainingModel();

                    training.TrainingId = reader.GetInt16(reader.GetOrdinal("TrainingId"));
                    training.TrainingName = reader.GetString(reader.GetOrdinal("TrainingName"));
                    training.Description = reader.GetString(reader.GetOrdinal("Description"));
                    training.Deadline = reader.GetDateTime(reader.GetOrdinal("Deadline"));
                    training.PriorityDepartment = reader.IsDBNull(reader.GetOrdinal("PriorityDepartment")) ? null : (int?)reader.GetInt16(reader.GetOrdinal("PriorityDepartment"));
                    training.PriorityDepartmentName = reader.IsDBNull(reader.GetOrdinal("DepartmentName")) ? null : reader.GetString(reader.GetOrdinal("DepartmentName"));
                    training.Capacity = reader.GetInt16(reader.GetOrdinal("Capacity"));

                    trainingList.Add(training);
                }
            }
            return trainingList;
        }

        public async Task<IEnumerable<TrainingModel>> GetAllByUserAsync(int userId)
        {
            TrainingModel training;
            List<TrainingModel> trainingsList = new List<TrainingModel>();

            List<SqlParameter> parameters = new List<SqlParameter>() { new SqlParameter("@UserId", userId) };

            const string GetTrainingsByUserQuery =
              @"SELECT t.*, e.[UserId], e.[TrainingId]
                FROM [dbo].[Enrollment] as e
                INNER JOIN [dbo].[Training] as t
                ON e.[TrainingId] = t.[TrainingId]
                WHERE e.[UserId] = @UserId;";

            using (SqlDataReader reader = await DbCommand.GetDataWithConditionsAsync(GetTrainingsByUserQuery, parameters))
            {
                while (reader.Read())
                {
                    training = new TrainingModel()
                    {
                        TrainingId = reader.GetInt16(reader.GetOrdinal("TrainingId")),
                        TrainingName = reader.GetString(reader.GetOrdinal("TrainingName")),
                        Description = reader.GetString(reader.GetOrdinal("Description")),
                        Deadline = reader.GetDateTime(reader.GetOrdinal("Deadline")),
                        PriorityDepartment = reader.IsDBNull(reader.GetOrdinal("PriorityDepartment")) ? null : (int?)reader.GetInt16(reader.GetOrdinal("PriorityDepartment")),
                        PriorityDepartmentName = reader.IsDBNull(reader.GetOrdinal("DepartmentName")) ? null : reader.GetString(reader.GetOrdinal("DepartmentName")),
                        Capacity = reader.GetInt16(reader.GetOrdinal("Capacity"))
                    };
                    trainingsList.Add(training);
                }
            }
            return trainingsList;
        }

        public async Task<IEnumerable<TrainingModel>> GetNotEnrolledTrainingsAsync(int userId)
        {
            TrainingModel training;
            List<TrainingModel> trainingsList = new List<TrainingModel>();
            List<SqlParameter> parameters = new List<SqlParameter>() { new SqlParameter("@UserId", userId) };

            const string GetNotEnrolledTrainings =
                @"SELECT *
                FROM Training t
                LEFT JOIN [dbo].[Department] AS d 
                ON t.[PriorityDepartment] = d.DepartmentId
                WHERE NOT EXISTS (
                    SELECT 1
                    FROM Enrollment e
                    WHERE e.UserId = @UserId
                        AND e.TrainingId = t.TrainingId
                )
                ORDER BY t.TrainingName ASC;";

            using (SqlDataReader reader = await DbCommand.GetDataWithConditionsAsync(GetNotEnrolledTrainings, parameters))
            {
                while (reader.Read())
                {
                    training = new TrainingModel()
                    {
                        TrainingId = reader.GetInt16(reader.GetOrdinal("TrainingId")),
                        TrainingName = reader.GetString(reader.GetOrdinal("TrainingName")),
                        Description = reader.GetString(reader.GetOrdinal("Description")),
                        Deadline = reader.GetDateTime(reader.GetOrdinal("Deadline")),
                        PriorityDepartment = reader.IsDBNull(reader.GetOrdinal("PriorityDepartment")) ? null : (int?)reader.GetInt16(reader.GetOrdinal("PriorityDepartment")),
                        PriorityDepartmentName = reader.IsDBNull(reader.GetOrdinal("DepartmentName")) ? null : reader.GetString(reader.GetOrdinal("DepartmentName")),
                        Capacity = reader.GetInt16(reader.GetOrdinal("Capacity"))
                    };
                    trainingsList.Add(training);
                }
            }
            return trainingsList;
        }

        public async Task<IEnumerable<TrainingModel>> GetByDeadlineAsync()
        {
            TrainingModel training;
            List<TrainingModel> trainingsList = new List<TrainingModel>();

            const string GetTrainingsByDeadline =
                @"SELECT [TrainingId]
                FROM [dbo].[Training]
                WHERE [Deadline] <= GETDATE();";

            using (SqlDataReader reader = await DbCommand.GetDataAsync(GetTrainingsByDeadline))
            {
                while (reader.Read())
                {
                    training = new TrainingModel()
                    {
                        TrainingId = reader.GetInt16(reader.GetOrdinal("TrainingId"))
                    };
                    trainingsList.Add(training);
                }
            }
            return trainingsList;
        }
    }
}
