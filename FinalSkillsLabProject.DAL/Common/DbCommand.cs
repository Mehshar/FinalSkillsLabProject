using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Threading.Tasks;

namespace FinalSkillsLabProject.DAL.Common
{
    public static class DbCommand
    {
        public static async Task<SqlDataReader> GetDataAsync(string query)
        {
            DAL dal = new DAL();
            SqlCommand cmd = new SqlCommand(query, dal.Connection)
            {
                CommandType = CommandType.Text
            };

            return await cmd.ExecuteReaderAsync(CommandBehavior.CloseConnection);
        }

        public static async Task<SqlDataReader> GetDataWithConditionsAsync(string query, List<SqlParameter> parameters)
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
            return await cmd.ExecuteReaderAsync(CommandBehavior.CloseConnection);
        }

        public static async Task<int> InsertUpdateDataAsync(string query, List<SqlParameter> parameters)
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
                numOfRowsAffected = await cmd.ExecuteNonQueryAsync();

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

        public static async Task<int> DeleteDataAsync(string query, SqlParameter parameter)
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

                rowsAffected = await cmd.ExecuteNonQueryAsync();
            }
            dal.CloseConnection();
            return rowsAffected;
        }
    }
}
