using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Dapper;

namespace SmartPOS.WinForms.DAL.Data
{
    public class DbHelper
    {
        public IEnumerable<T> Query<T>(string sql, object param = null)
        {
            using (SqlConnection connection = DbConnectionFactory.CreateConnection())
            {
                connection.Open();
                return connection.Query<T>(sql, param);
            }
        }

        public T QueryFirstOrDefault<T>(string sql, object param = null)
        {
            using (SqlConnection connection = DbConnectionFactory.CreateConnection())
            {
                connection.Open();
                return connection.QueryFirstOrDefault<T>(sql, param);
            }
        }

        public int Execute(string sql, object param = null)
        {
            using (SqlConnection connection = DbConnectionFactory.CreateConnection())
            {
                connection.Open();
                return connection.Execute(sql, param);
            }
        }

        public object ExecuteScalar(string sql, object param = null)
        {
            using (SqlConnection connection = DbConnectionFactory.CreateConnection())
            {
                connection.Open();
                return connection.ExecuteScalar(sql, param);
            }
        }
    }
}