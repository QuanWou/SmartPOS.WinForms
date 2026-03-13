using System;
using System.Data;
using System.Data.SqlClient;

namespace SmartPOS.WinForms.DAL.Data
{
    public class DbTransactionScope : IDisposable
    {
        public SqlConnection Connection { get; private set; }

        public SqlTransaction Transaction { get; private set; }

        public DbTransactionScope()
        {
            Connection = DbConnectionFactory.CreateConnection();
            Connection.Open();
            Transaction = Connection.BeginTransaction(IsolationLevel.ReadCommitted);
        }

        public void Commit()
        {
            if (Transaction != null)
            {
                Transaction.Commit();
            }
        }

        public void Rollback()
        {
            if (Transaction != null)
            {
                Transaction.Rollback();
            }
        }

        public void Dispose()
        {
            if (Transaction != null)
            {
                Transaction.Dispose();
                Transaction = null;
            }

            if (Connection != null)
            {
                if (Connection.State == ConnectionState.Open)
                {
                    Connection.Close();
                }

                Connection.Dispose();
                Connection = null;
            }
        }
    }
}