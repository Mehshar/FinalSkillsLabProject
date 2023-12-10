using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;

namespace FinalSkillsLabProject.DAL.Common
{
    public static class DbCommand
    {
        public static SqlDataReader GetData(string query)
        {
            DAL dal = new DAL();
            SqlCommand cmd = new SqlCommand(query, dal.Connection)
            {
                CommandType = CommandType.Text
            };

            return cmd.ExecuteReader(CommandBehavior.CloseConnection);
        }

        public static SqlDataReader GetDataWithConditions(string query, List<SqlParameter> parameters)
        {
            DAL dal = new DAL();
            SqlCommand cmd = new SqlCommand(query, dal.Connection);
            cmd.CommandType = CommandType.Text;

            if (parameters != null)
            {
                parameters.ForEach(parameter =>
                {
                    cmd.Parameters.AddWithValue(parameter.ParameterName, parameter.Value);
                });
            }
            return cmd.ExecuteReader(CommandBehavior.CloseConnection);
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
                        if (parameter.Direction == ParameterDirection.Output)
                        {
                            SqlParameter outputParameter = cmd.Parameters.Add(parameter.ParameterName, parameter.SqlDbType);
                            outputParameter.Direction = ParameterDirection.Output;
                        }

                        else
                        {
                            cmd.Parameters.AddWithValue(parameter.ParameterName, parameter.Value);
                        }
                    });
                }
                numOfRowsAffected = cmd.ExecuteNonQuery();

                foreach (var parameter in parameters)
                {
                    if (parameter.Direction == ParameterDirection.Output)
                    {
                        parameter.Value = cmd.Parameters[parameter.ParameterName].Value;
                    }
                }
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
