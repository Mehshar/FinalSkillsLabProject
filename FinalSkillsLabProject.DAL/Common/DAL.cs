using System.Data.SqlClient;
using System.Configuration;

namespace FinalSkillsLabProject.DAL.Common
{
    public class DAL
    {
        public string _connectionString = ConfigurationManager.ConnectionStrings["DefaultConnectionString"].ConnectionString;
        public SqlConnection Connection;

        public DAL()
        {
            Connection = new SqlConnection(_connectionString);
            OpenConnection();
        }

        public void OpenConnection()
        {
            if (Connection.State == System.Data.ConnectionState.Open)
            {
                Connection.Close();
            }
            Connection.Open();
        }

        public void CloseConnection()
        {
            if (Connection != null && Connection.State == System.Data.ConnectionState.Open)
            {
                Connection.Close();
                Connection.Dispose();
            }
        }
    }
}
