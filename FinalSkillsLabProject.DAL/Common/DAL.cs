using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            try
            {
                if (Connection.State == System.Data.ConnectionState.Open)
                {
                    Connection.Close();
                }
                Connection.Open();
            }

            catch (SqlException e)
            {
                throw e;
            }
        }

        public void CloseConnection()
        {
            try
            {
                if (Connection != null && Connection.State == System.Data.ConnectionState.Open)
                {
                    Connection.Close();
                    Connection.Dispose();
                }
            }

            catch (SqlException e)
            {
                throw e;
            }
        }
    }
}
