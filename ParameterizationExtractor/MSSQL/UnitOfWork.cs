using Quipu.ParameterizationExtractor.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Threading;
using System.Data.SqlClient;

namespace Quipu.ParameterizationExtractor.MSSQL
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly SqlConnection _sqlConnection;
        private SqlTransaction _transaction;
        public UnitOfWork(string connectionString)
        {
            _sqlConnection = new SqlConnection(connectionString);
            _sqlConnection.Open();
        }

        private bool IsOpened { get; set; }

        public void BeginTran()
        {
            _transaction = _transaction ?? _sqlConnection.BeginTransaction();
        }

        public void Commit()
        {
            if (_transaction != null)
                _transaction.Commit();
        }

        public void Dispose()
        {
            _sqlConnection.Close();

            if (_transaction != null)
                _transaction.Dispose();

            if (_sqlConnection != null)
                _sqlConnection.Dispose();
        }

        public Task<SqlDataReader> ExecuteReaderAsync(string sql)
        {
            var com = new SqlCommand(sql, _sqlConnection, _transaction);

            return com.ExecuteReaderAsync();
        }
      
        public Task<DataTable> GetSchemaAsync(string collectionName)
        {
            return Task.Run(() => _sqlConnection.GetSchema(collectionName));
        }

        public Task<DataTable> GetSchemaAsync(string collectionName, string[] restrictionValues)
        {
            return Task.Run(() => _sqlConnection.GetSchema(collectionName, restrictionValues));
        }

        public void Rollback()
        {
            if (_transaction != null)
                _transaction.Rollback();
        }
    }
}
