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
        private const string _InsertTrainingQuery =
            @"BEGIN TRANSACTION;

            DECLARE @training_key INT            

            INSERT INTO [dbo].[Training] ([TrainingName], [Description], [Deadline], [PriorityDepartment], [Capacity])
            SELECT @TrainingName, @Description, @Deadline, @PriorityDepartment, @Capacity;

            SELECT @training_key = @@IDENTITY

            COMMIT;";

        private const string _UpdateTrainingQuery =
            @"BEGIN TRANSACTION;

            UPDATE [dbo].[Training]
            SET [TrainingName] = @TrainingName,
	            [Description] = @Description,
	            [Deadline] = @Deadline,
	            [PriorityDepartment] = @PriorityDepartment,
	            [Capacity] = @Capacity
            WHERE [TrainingId] = @TrainingId;

            COMMIT;";

        private const string _DeleteTrainingQuery =
            @"BEGIN TRANSACTION;

            DELETE FROM [dbo].[Training]
            WHERE [TrainingId] = @TrainingId;

            COMMIT;";

        private const string _GetTrainingQuery =
            @"
            BEGIN TRANSACTION;

            SELECT *
            FROM [dbo].[Training]
            WHERE [TrainingId] = @TrainingId;

            COMMIT;";

        private const string _GetAllTrainingsQuery =
            @"BEGIN TRANSACTION;

            SELECT *
            FROM [dbo].[Training] as t
            LEFT JOIN [dbo].[Department] as d
            ON t.[PriorityDepartment] = d.DepartmentId;

            COMMIT;";

        private const string _GetTrainingsByUserQuery =
            @"BEGIN TRANSACTION;

            SELECT t.*, e.[UserId], e.[TrainingId]
            FROM [dbo].[Enrollment] as e
            INNER JOIN [dbo].[Training] as t
            ON e.[TrainingId] = t.[TrainingId]
            WHERE e.[UserId] = @UserId;

            COMMIT;";

        public void Add(TrainingModel training)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();

            parameters.Add(new SqlParameter("@TrainingName", training.TrainingName));
            parameters.Add(new SqlParameter("@Description", training.Description));
            parameters.Add(new SqlParameter("@Deadline", training.Deadline));
            parameters.Add(new SqlParameter("@PriorityDepartment", training.PriorityDepartment));
            parameters.Add(new SqlParameter("@Capacity", training.Capacity));

            DbCommand.InsertUpdateData(_InsertTrainingQuery, parameters);
        }

        public void Update(TrainingModel training)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();

            //foreach(var property in training.GetType().GetProperties())
            //{
            //    parameters.Add(new SqlParameter($"@{property.Name}", property.GetValue(training)));
            //}

            parameters.Add(new SqlParameter("@TrainingName", training.TrainingName));
            parameters.Add(new SqlParameter("@Description", training.Description));
            parameters.Add(new SqlParameter("@Deadline", training.Deadline));
            parameters.Add(new SqlParameter("@PriorityDepartment", training.PriorityDepartment));
            parameters.Add(new SqlParameter("@Capacity", training.Capacity));
            parameters.Add(new SqlParameter("@TrainingId", training.TrainingId));

            DbCommand.InsertUpdateData(_UpdateTrainingQuery, parameters);
        }

        public bool Delete(int trainingId)
        {
            SqlParameter parameter = new SqlParameter("@TrainingId", trainingId);
            return DbCommand.DeleteData(_DeleteTrainingQuery, parameter) > 0;
        }

        public TrainingModel Get(int trainingId)
        {
            TrainingModel training = new TrainingModel();
            List<SqlParameter> parameters = new List<SqlParameter>();

            parameters.Add(new SqlParameter("@TrainingId", trainingId));

            DataTable dt = DbCommand.GetDataWithConditions(_GetTrainingQuery, parameters);

            DataRow row = dt.Rows[0];
            training.TrainingId = trainingId;
            training.TrainingName = row["TrainingName"].ToString();
            training.Description = row["Description"].ToString();
            training.Deadline = (DateTime)row["Deadline"];
            if (row["PriorityDepartment"] != DBNull.Value)
            {
                training.PriorityDepartment = int.Parse(row["PriorityDepartment"].ToString());
                training.PriorityDepartmentName = row["DepartmentName"].ToString();
            }
            else
            {
                training.PriorityDepartment = null;
                training.PriorityDepartmentName = null;
            }
            training.Capacity = int.Parse(row["Capacity"].ToString());

            return training;
        }
        public IEnumerable<TrainingModel> GetAll()
        {
            TrainingModel training;
            List<TrainingModel> trainingList = new List<TrainingModel>();

            DataTable dt = DbCommand.GetData(_GetAllTrainingsQuery);

            foreach (DataRow row in dt.Rows)
            {
                training = new TrainingModel();

                training.TrainingId = int.Parse(row["TrainingId"].ToString());
                training.TrainingName = row["TrainingName"].ToString();
                training.Description = row["Description"].ToString();
                training.Deadline = (DateTime)row["Deadline"];
                if (row["PriorityDepartment"] != DBNull.Value)
                {
                    training.PriorityDepartment = int.Parse(row["PriorityDepartment"].ToString());
                    training.PriorityDepartmentName = row["DepartmentName"].ToString();
                }
                else
                {
                    training.PriorityDepartment = null;
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

            DataTable dt = DbCommand.GetDataWithConditions(_GetTrainingsByUserQuery, parameters);

            foreach (DataRow row in dt.Rows)
            {
                training = new TrainingModel();

                training.TrainingId = int.Parse(row["TrainingId"].ToString());
                training.TrainingName = row["TrainingName"].ToString();
                training.Description = row["Description"].ToString();
                training.Deadline = (DateTime)row["Deadline"];

                if (row["PriorityDepartment"] != DBNull.Value)
                {
                    training.PriorityDepartment = int.Parse(row["PriorityDepartment"].ToString());
                    training.PriorityDepartmentName = row["DepartmentName"].ToString();
                }
                else
                {
                    training.PriorityDepartment = null;
                    training.PriorityDepartmentName = null;
                }
                training.Capacity = int.Parse(row["Capacity"].ToString());

                trainingsList.Add(training);
            }
            return trainingsList;
        }
    }
}
