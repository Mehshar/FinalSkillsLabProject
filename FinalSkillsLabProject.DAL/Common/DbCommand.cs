using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalSkillsLabProject.DAL.Common
{
    // DML
    // Insert, Update, Select and Delete operations
    public static class DbCommand
    {
        public static DataTable GetData(string query)
        {
            DAL dal = new DAL();
            DataTable dt = new DataTable();

            using (SqlCommand cmd = new SqlCommand(query, dal.Connection))
            {
                cmd.CommandType = CommandType.Text;
                using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                {
                    sda.Fill(dt);
                }
            }
            dal.CloseConnection();
            return dt;
        }

        public static DataTable GetDataWithConditions(string query, List<SqlParameter> parameters)
        {
            DAL dal = new DAL();
            DataTable dt = new DataTable();

            using (SqlCommand cmd = new SqlCommand(query, dal.Connection))
            {
                cmd.CommandType = CommandType.Text;

                if (parameters != null)
                {
                    parameters.ForEach(parameter =>
                    {
                        cmd.Parameters.AddWithValue(parameter.ParameterName, parameter.Value);
                    });
                }

                using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                {
                    sda.Fill(dt);
                }
            }
            dal.CloseConnection();
            return dt;
        }

        public static int InsertUpdateData(string query, List<SqlParameter> parameters)
        {
            DAL dal = new DAL();
            int numOfRowsAffected = 0;

            using (SqlCommand cmd = new SqlCommand(query, dal.Connection))
            {
                cmd.CommandType = CommandType.Text;

                if (parameters != null)
                {
                    parameters.ForEach(parameter =>
                    {
                        cmd.Parameters.AddWithValue(parameter.ParameterName, parameter.Value);
                    });
                }
                numOfRowsAffected = cmd.ExecuteNonQuery();
            }
            dal.CloseConnection();

            return numOfRowsAffected;
        }

        public static int DeleteData(string query, SqlParameter parameter)
        {
            DAL dal = new DAL();
            int rowsAffected = 0;

            using (SqlCommand cmd = new SqlCommand(query, dal.Connection))
            {
                cmd.CommandType = CommandType.Text;

                if (parameter != null)
                {
                    cmd.Parameters.AddWithValue(parameter.ParameterName, parameter.Value);
                }

                rowsAffected = cmd.ExecuteNonQuery();
            }
            dal.CloseConnection();
            return rowsAffected;
        }
    }
}
