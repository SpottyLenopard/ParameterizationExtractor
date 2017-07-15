using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Quipu.ParameterizationExtractor.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        void BeginTran();
        void Rollback();
        void Commit();

        Task<DataTable> GetSchemaAsync(string collectionName);
        Task<DataTable> GetSchemaAsync(string collectionName, string[] restrictionValues);       
        Task<SqlDataReader> ExecuteReaderAsync(string sql);
        string Database { get; }
        string DataSource { get; }
    }

    public interface IUnitOfWorkFactory
    {
        IUnitOfWork GetUnitOfWork();
        IUnitOfWork GetUnitOfWork(string source);
    }
}
