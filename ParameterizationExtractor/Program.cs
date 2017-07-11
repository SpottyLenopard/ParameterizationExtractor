using Quipu.ParameterizationExtractor.Interfaces;
using Quipu.ParameterizationExtractor.Model;
using Quipu.ParameterizationExtractor.MSSQL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Quipu.ParameterizationExtractor.Templates;
using Quipu.ParameterizationExtractor.Common;

namespace ParameterizationExtractor
{
    class Program
    {
        private static IUnitOfWorkFactory _uowFactory;
        private static ILog _log;
        static void Main(string[] args)
        {
            _uowFactory = new UnitOfWorkFactory();
            _log = new ConsoleLogger();
            MainAsync().Wait();
           
            Console.ReadKey();
        }

        static async Task MainAsync()
        {
            var pckg = new PackageTemplate();
            pckg.RootRecords.Add("ServiceJobSchedules", "where ServiceJobScheduleId = 789792");
            pckg.Exceptions.Add("Users", null);
            pckg.Exceptions.Add("ServiceJobItemRelationLog", null);
            pckg.Exceptions.Add("ServiceJobLog", null);
            pckg.Exceptions.Add("ServiceJobItemLog", null);
            pckg.Exceptions.Add("ApplicationServers", null);
            pckg.Exceptions.Add("ServiceJobSchedulesDependencies", null);
            //"ServiceJobItemRelationLog" "ServiceJobLog" "ServiceJobItemRelationLog"
            var DependentTables = GetDependentTables();
            var MetaData = GetMetaData(new MetaDataInitializer());

            await Task.WhenAll(DependentTables, MetaData);

            var schema = new MSSQLSourceSchema(DependentTables.Result, MetaData.Result);

            var builder = new DependencyBuilder(_uowFactory, schema, _log);

            var pTables = await builder.PrepareAsync(pckg);
            var sqlBuilder = new MSSqlBuilder();

            _log.Debug(sqlBuilder.Build(pTables));

        }


        private static async Task<IEnumerable<PDependentTable>> GetDependentTables()
        {
            var result = new List<PDependentTable>();

            using (var uof = _uowFactory.GetUnitOfWork())
            using (var dr = await uof.ExecuteReaderAsync(MSSQLSourceSchema.sqlFKs))
            {
                var dt = new DataTable();
                dt.Load(dr);

                foreach (DataRow r in dt.Rows)
                {
                    var item = new PDependentTable
                    {
                        Name = r["Name"].ToString(),
                        ParentColumn = r["ParentColumn"].ToString(),
                        ParentTable = r["ParentTable"].ToString(),
                        ReferencedColumn = r["ReferencedColumn"].ToString(),
                        ReferencedTable = r["ReferencedTable"].ToString()
                    };

                    result.Add(item);
                }
            }

            return result;
        }

        private static async Task<IEnumerable<PTableMetadata>> GetMetaData(IMetaDataInitializer initializer)
        {
            var result = new List<PTableMetadata>();

            using (var uof = _uowFactory.GetUnitOfWork())
            {
                var metaTables = uof.GetSchemaAsync("Tables");

                var metaColumns = uof.GetSchemaAsync("Columns");

                var indexes = uof.ExecuteReaderAsync(MSSQLSourceSchema.sqlPKColumns);

                await Task.WhenAll(metaColumns, metaTables, indexes);

                var dt = new DataTable();
                dt.Load(indexes.Result);

                var iList = dt.Rows.Cast<DataRow>();

                foreach (DataRow t in metaTables.Result.Rows)
                {
                    var pTab = new PTableMetadata() { TableName = t["table_name"].ToString() };
                    //var pkField = iList.FirstOrDefault(_ => _["TableName"].ToString() == pTab.TableName)?["ColumnName"].ToString();
                    var indx = iList.Where(_ => _["TableName"].ToString() == pTab.TableName).ToList();

                    foreach (DataRow c in metaColumns.Result.Rows.Cast<DataRow>().Where(_ => _["table_name"].ToString() == pTab.TableName))
                    {
                        var field = initializer.InitTableMetaData(c, indx);
                      
                        pTab.Add(field);
                       
                    }

                    result.Add(pTab);
                }
            }

            return result;
        }
    }
}
