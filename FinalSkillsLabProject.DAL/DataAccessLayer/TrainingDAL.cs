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
    public class TrainingDAL : ITrainingDAL
    {
        public void Add(TrainingModel training, List<int> prerequisitesList)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();

            parameters.Add(new SqlParameter("@TrainingName", training.TrainingName));
            parameters.Add(new SqlParameter("@Description", training.Description));
            parameters.Add(new SqlParameter("@Deadline", training.Deadline));
            parameters.Add(new SqlParameter("@PriorityDepartment", training.PriorityDepartment));
            parameters.Add(new SqlParameter("@Capacity", training.Capacity));
            
            if(prerequisitesList == null)
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

            DbCommand.InsertUpdateData(InsertTrainingQuery, parameters);
        }

        public void Update(TrainingModel training)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();

            parameters.Add(new SqlParameter("@TrainingName", training.TrainingName));
            parameters.Add(new SqlParameter("@Description", training.Description));
            parameters.Add(new SqlParameter("@Deadline", training.Deadline));
            parameters.Add(new SqlParameter("@PriorityDepartment", Convert.ToInt16(training.PriorityDepartment)));
            parameters.Add(new SqlParameter("@Capacity", Convert.ToInt16(training.Capacity)));
            parameters.Add(new SqlParameter("@TrainingId", Convert.ToInt16(training.TrainingId)));

            const string UpdateTrainingQuery =
                @"BEGIN TRANSACTION;

                UPDATE [dbo].[Training]
                SET [TrainingName] = @TrainingName,
	                [Description] = @Description,
	                [Deadline] = @Deadline,
	                [PriorityDepartment] = @PriorityDepartment,
	                [Capacity] = @Capacity
                WHERE [TrainingId] = @TrainingId;

                COMMIT;";

            DbCommand.InsertUpdateData(UpdateTrainingQuery, parameters);
        }

        public bool Delete(int trainingId)
        {
            SqlParameter parameter = new SqlParameter("@TrainingId", trainingId);

            const string DeleteTrainingQuery =
                @"BEGIN TRANSACTION;

                DELETE FROM [dbo].[Training]
                WHERE [TrainingId] = @TrainingId;

                COMMIT;";

            return DbCommand.DeleteData(DeleteTrainingQuery, parameter) > 0;
        }

        public TrainingModel Get(int trainingId)
        {
            TrainingModel training = new TrainingModel();
            List<SqlParameter> parameters = new List<SqlParameter>();

            parameters.Add(new SqlParameter("@TrainingId", trainingId));

            const string GetTrainingQuery =
                @"
                BEGIN TRANSACTION;

                SELECT *
                FROM [dbo].[Training] AS t
                LEFT JOIN [dbo].[Department] AS d
                ON t.[PriorityDepartment] = d.[DepartmentId]
                WHERE [TrainingId] = @TrainingId;

                COMMIT;";

            DataTable dt = DbCommand.GetDataWithConditions(GetTrainingQuery, parameters);

            DataRow row = dt.Rows[0];
            training.TrainingId = trainingId;
            training.TrainingName = row["TrainingName"].ToString();
            training.Description = row["Description"].ToString();
            training.Deadline = (DateTime)row["Deadline"];
            training.PriorityDepartment = int.Parse(row["PriorityDepartment"].ToString());
            if (row["DepartmentName"] != DBNull.Value)
            {
                training.PriorityDepartmentName = row["DepartmentName"].ToString();
            }
            else
            {
                training.PriorityDepartmentName = null;
            }
            training.Capacity = int.Parse(row["Capacity"].ToString());

            return training;
        }
        public IEnumerable<TrainingModel> GetAll()
        {
            TrainingModel training;
            List<TrainingModel> trainingList = new List<TrainingModel>();

            const string GetAllTrainingsQuery =
                @"BEGIN TRANSACTION;

                SELECT *
                FROM [dbo].[Training] as t
                LEFT JOIN [dbo].[Department] as d
                ON t.[PriorityDepartment] = d.DepartmentId;

                COMMIT;";

            DataTable dt = DbCommand.GetData(GetAllTrainingsQuery);

            foreach (DataRow row in dt.Rows)
            {
                training = new TrainingModel();

                training.TrainingId = int.Parse(row["TrainingId"].ToString());
                training.TrainingName = row["TrainingName"].ToString();
                training.Description = row["Description"].ToString();
                training.Deadline = (DateTime)row["Deadline"];
                training.PriorityDepartment = int.Parse(row["PriorityDepartment"].ToString());
                if (row["DepartmentName"] != DBNull.Value)
                {
                    training.PriorityDepartmentName = row["DepartmentName"].ToString();
                }
                else
                {
                    training.PriorityDepartmentName = null;
                }
                training.Capacity = int.Parse(row["Capacity"].ToString());

                trainingList.Add(training);
            }
            return trainingList;
        }

        public IEnumerable<TrainingModel> GetAllByUser(int userId)
        {
            TrainingModel training;
            List<TrainingModel> trainingsList = new List<TrainingModel>();

            List<SqlParameter> parameters = new List<SqlParameter>() { new SqlParameter("@UserId", userId) };

            const string GetTrainingsByUserQuery =
                @"BEGIN TRANSACTION;

                SELECT t.*, e.[UserId], e.[TrainingId]
                FROM [dbo].[Enrollment] as e
                INNER JOIN [dbo].[Training] as t
                ON e.[TrainingId] = t.[TrainingId]
                WHERE e.[UserId] = @UserId;

                COMMIT;";

            DataTable dt = DbCommand.GetDataWithConditions(GetTrainingsByUserQuery, parameters);

            foreach (DataRow row in dt.Rows)
            {
                training = new TrainingModel();

                training.TrainingId = int.Parse(row["TrainingId"].ToString());
                training.TrainingName = row["TrainingName"].ToString();
                training.Description = row["Description"].ToString();
                training.Deadline = (DateTime)row["Deadline"];
                training.PriorityDepartment = int.Parse(row["PriorityDepartment"].ToString());
                if (row["DepartmentName"] != DBNull.Value)
                {
                    training.PriorityDepartmentName = row["DepartmentName"].ToString();
                }
                else
                {
                    training.PriorityDepartmentName = null;
                }
                training.Capacity = int.Parse(row["Capacity"].ToString());

                trainingsList.Add(training);
            }
            return trainingsList;
        }
    }
}
