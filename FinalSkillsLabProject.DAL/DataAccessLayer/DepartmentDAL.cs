using System;
using System.Collections.Generic;
using FinalSkillsLabProject.DAL.Common;
using System.Data.SqlClient;
using System.Data;
using FinalSkillsLabProject.Common.Models;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FinalSkillsLabProject.DAL.Interfaces;

namespace FinalSkillsLabProject.DAL.DataAccessLayer
{
    public class DepartmentDAL : IDepartmentDAL
    {
        private const string _InsertDepartmentQuery =
            @"BEGIN TRANSACTION;

            DECLARE @department_key INT            

            INSERT INTO [dbo].[Department] ([DepartmentName])
            SELECT @DepartmentName;

            SELECT @department_key = @@IDENTITY

            COMMIT;";

        private const string _UpdateDepartmentQuery =
            @"BEGIN TRANSACTION;

            UPDATE [dbo].[Department]
            SET [DepartmentName] = @DepartmentName
            WHERE [DepartmentId] = @DepartmentId;

            COMMIT;";

        private const string _GetDepartmentQuery =
            @"BEGIN TRANSACTION;
            
            SELECT *
            FROM [dbo].[Department]
            WHERE [DepartmentId] = @DepartmentId;

            COMMIT;";

        private const string _GetAllDepartmentsQuery =
            @"BEGIN TRANSACTION;

            SELECT *
            FROM [dbo].[Department] 

            COMMIT;";

        public bool Add(DepartmentModel department)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();

            parameters.Add(new SqlParameter("@DepartmentName", department.DepartmentName));
            return DbCommand.InsertUpdateData(_InsertDepartmentQuery, parameters) > 0;
        }
        public bool Update(DepartmentModel department)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();

            parameters.Add(new SqlParameter("@DepartmentId", department.DepartmentId));
            parameters.Add(new SqlParameter("@DepartmentName", department.DepartmentName));

            return DbCommand.InsertUpdateData(_UpdateDepartmentQuery, parameters) > 0;
        }
        //public void Delete(DepartmentModel department) { }
        public DepartmentModel Get(int departmentId)
        {
            DepartmentModel department = new DepartmentModel();

            List<SqlParameter> parameters = new List<SqlParameter>();

            parameters.Add(new SqlParameter("@DepartmentId", departmentId));

            DataTable dt = DbCommand.GetDataWithConditions(_GetDepartmentQuery, parameters);

            DataRow row = dt.Rows[0];
            department.DepartmentId = departmentId;
            department.DepartmentName = row["DepartmentName"].ToString();
            return department;
        }
        public IEnumerable<DepartmentModel> GetAll()
        {
            DepartmentModel department;
            DataTable dt = DbCommand.GetData(_GetAllDepartmentsQuery);

            List<DepartmentModel> departmentsList = new List<DepartmentModel>();

            foreach (DataRow row in dt.Rows)
            {
                department = new DepartmentModel();

                department.DepartmentId = int.Parse(row["DepartmentId"].ToString());
                department.DepartmentName = row["DepartmentName"].ToString();

                departmentsList.Add(department);
            }
            return departmentsList;
        }
    }
}
