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
        private const string _GetPrerequisitesByTrainingQuery =
            @"BEGIN TRANSACTION;

            SELECT p.*
            FROM [dbo].[Training_Prerequisite] AS tp
            INNER JOIN [dbo].[Prerequisite] AS p
            ON tp.[PrerequisiteId] = p.[PrerequisiteId]
            WHERE tp.[TrainingId] = @TrainingId;

            COMIMT;";

        private const string _InsertPrerequisiteQuery =
            @"BEGIN TRANSACTION;

            DECLARE @prerequisite_key INT 

            INSERT INTO [dbo].[Prerequisite] ([Type], [Description])
            SELECT @Type, @Description;

            SELECT @prerequisite_key = @@IDENTITY

            INSERT INTO [dbo].[Training_Prerequisite] ([TrainingId], [PrerequisiteId])
            SELECT @TrainingId, @prerequisite_key;

            COMMIT;";

        private const string _UpdatePrerequisiteQuery =
            @"BEGIN TRANSACTION;

            UPDATE [dbo].[Prerequisite]
            SET [Type] = @Type,
	            [Description] = @Description
            WHERE [PrerequisiteId] = @PrerequisiteId;

            COMMIT;";

        private const string _DeletePrerequisiteQuery =
            @"BEGIN TRANSACTION;

            DELETE FROM [dbo].[Prerequisite]
            WHERE [PrerequisiteId] = @PrerequisiteId;

            COMMIT;";

        private const string _GetAllPrerequisitesQuery =
            @"BEGIN TRANSACTION;

            SELECT *
            FROM [dbo].[Prerequisite];

            COMMIT;";

        public void Add(PrerequisiteModel prerequisite, int trainingId)
        {
            List<SqlParameter> parameters = new List<SqlParameter>()
            {
                new SqlParameter("@Type", prerequisite.Type),
                new SqlParameter("@Description", prerequisite.Description),
                new SqlParameter("@TrainingId", trainingId)
            };

            DbCommand.InsertUpdateData(_InsertPrerequisiteQuery, parameters);
        }

        public void Update(PrerequisiteModel prerequisite)
        {
            List<SqlParameter> parameters = new List<SqlParameter>()
            {
                new SqlParameter("@Type", prerequisite.Type),
                new SqlParameter("@Description", prerequisite.Description),
                new SqlParameter("@PrerequisiteId", prerequisite.PrerequisiteId)
            };

            DbCommand.InsertUpdateData(_UpdatePrerequisiteQuery, parameters);
        }

        public bool Delete(int prerequisiteId)
        {
            SqlParameter parameter = new SqlParameter("@PrerequisiteId", prerequisiteId);

            return DbCommand.DeleteData(_DeletePrerequisiteQuery, parameter) > 0;
        }

        public IEnumerable<PrerequisiteModel> GetAll()
        {
            PrerequisiteModel prerequisite;
            List<PrerequisiteModel> prerequisitesList = new List<PrerequisiteModel>();

            DataTable dt = DbCommand.GetData(_GetAllPrerequisitesQuery);

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

            DataTable dt = DbCommand.GetDataWithConditions(_GetPrerequisitesByTrainingQuery, parameters);

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
