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
    public class PrerequisiteDAL : IPrerequisiteDAL
    {
        public void Add(PrerequisiteModel prerequisite, int trainingId)
        {
            List<SqlParameter> parameters = new List<SqlParameter>()
            {
                new SqlParameter("@Type", prerequisite.Type),
                new SqlParameter("@Description", prerequisite.Description),
                new SqlParameter("@TrainingId", trainingId)
            };

            const string InsertPrerequisiteQuery =
                @"BEGIN TRANSACTION;

                DECLARE @prerequisite_key INT 

                INSERT INTO [dbo].[Prerequisite] ([Type], [Description])
                SELECT @Type, @Description;

                SELECT @prerequisite_key = @@IDENTITY

                INSERT INTO [dbo].[Training_Prerequisite] ([TrainingId], [PrerequisiteId])
                SELECT @TrainingId, @prerequisite_key;

                COMMIT;";

            DbCommand.InsertUpdateData(InsertPrerequisiteQuery, parameters);
        }

        public void Update(PrerequisiteModel prerequisite)
        {
            List<SqlParameter> parameters = new List<SqlParameter>()
            {
                new SqlParameter("@Type", prerequisite.Type),
                new SqlParameter("@Description", prerequisite.Description),
                new SqlParameter("@PrerequisiteId", prerequisite.PrerequisiteId)
            };

            const string UpdatePrerequisiteQuery =
              @"UPDATE [dbo].[Prerequisite]
                SET [Type] = @Type,
	                [Description] = @Description
                WHERE [PrerequisiteId] = @PrerequisiteId;";

            DbCommand.InsertUpdateData(UpdatePrerequisiteQuery, parameters);
        }

        public bool Delete(int prerequisiteId)
        {
            SqlParameter parameter = new SqlParameter("@PrerequisiteId", prerequisiteId);

            const string DeletePrerequisiteQuery =
              @"DELETE FROM [dbo].[Prerequisite]
                WHERE [PrerequisiteId] = @PrerequisiteId;";

            return DbCommand.DeleteData(DeletePrerequisiteQuery, parameter) > 0;
        }

        public IEnumerable<PrerequisiteModel> GetAll()
        {
            PrerequisiteModel prerequisite;
            List<PrerequisiteModel> prerequisitesList = new List<PrerequisiteModel>();

            const string GetAllPrerequisitesQuery =
              @"SELECT *
                FROM [dbo].[Prerequisite];";

            DataTable dt = DbCommand.GetData(GetAllPrerequisitesQuery);

            foreach (DataRow row in dt.Rows)
            {
                prerequisite = new PrerequisiteModel()
                {
                    PrerequisiteId = int.Parse(row["PrerequisiteId"].ToString()),
                    Type = row["Type"].ToString(),
                    Description = row["Description"].ToString()
                };

                prerequisitesList.Add(prerequisite);
            }
            return prerequisitesList;
        }

        public IEnumerable<PrerequisiteModel> GetAllByTraining(int trainingId)
        {
            PrerequisiteModel prerequisite;
            List<PrerequisiteModel> prerequisitesList = new List<PrerequisiteModel>();

            List<SqlParameter> parameters = new List<SqlParameter>()
            {
                new SqlParameter("@TrainingId", trainingId)
            };

            const string GetPrerequisitesByTrainingQuery =
              @"SELECT p.*
                FROM [dbo].[Training_Prerequisite] AS tp
                INNER JOIN [dbo].[Prerequisite] AS p
                ON tp.[PrerequisiteId] = p.[PrerequisiteId]
                WHERE tp.[TrainingId] = @TrainingId;";

            DataTable dt = DbCommand.GetDataWithConditions(GetPrerequisitesByTrainingQuery, parameters);

            foreach (DataRow row in dt.Rows)
            {
                prerequisite = new PrerequisiteModel();

                prerequisite.PrerequisiteId = int.Parse(row["PrerequisiteId"].ToString());
                prerequisite.Type = row["Type"].ToString();
                prerequisite.Description = row["Description"].ToString();

                prerequisitesList.Add(prerequisite);
            }
            return prerequisitesList;
        }
    }
}
