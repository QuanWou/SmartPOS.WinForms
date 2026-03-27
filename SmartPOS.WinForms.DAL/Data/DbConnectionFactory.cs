using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Configuration;
using System.Data.SqlClient;

namespace SmartPOS.WinForms.DAL.Data
{
    public static class DbConnectionFactory
    {
        public static SqlConnection CreateConnection()
        {
            string connectionString = ConfigurationManager
                .ConnectionStrings["SmartPOSConnection"]
                .ConnectionString;

            DatabaseSchemaInitializer.EnsureInitialized(connectionString);
            return new SqlConnection(connectionString);
        }
    }
}
