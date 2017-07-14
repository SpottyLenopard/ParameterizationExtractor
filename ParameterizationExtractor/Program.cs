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
    public class Program
    {
        private static ILog _log;        
        public static IUnitOfWorkFactory _uowFactory
        {
            get { return UnitOfWorkFactory.GetInstance(); }
        }
        public static GlobalExtractConfiguration GlobalConfiguration
        {
            get { return GlobalExtractConfiguration.GetInstance(); }
        }

        static void FillConfigForDebug()
        {
            GlobalConfiguration.DefaultExtractStrategy = new OnlyChildrenExtractStrategy();
            GlobalConfiguration.FieldsToExclude.Add("CreatorId");
            GlobalConfiguration.FieldsToExclude.Add("Created");

            GlobalConfiguration.UniqueColums.Add("BusinessProcessSteps", new UniqueColumnsCollection() { "BPTypeCode", "StepStartStatusId", "StepEndStatusId" });
            GlobalConfiguration.UniqueColums.Add("bpProcessingConditions", new UniqueColumnsCollection() { "ConditionText" });
        }

        static void Main(string[] args)
        {
            _log = new ConsoleLogger();
            FillConfigForDebug();
            MainAsync().Wait();
           
            Console.ReadKey();
        }

        static PackageTemplate CreateDebugPackage()
        {
            var pckg = new PackageTemplate();
            //pckg.RootRecords.Add("ServiceJobSchedules", "where ServiceJobScheduleId = 789792");
            //pckg.RootRecords.Add(new RecordToExtract("ServiceJobs", "789800"));
            //pckg.UniqueColums.Add("BusinessProcessSteps", new UniqueColumnsCollection() { "BPTypeCode", "StepStartStatusId", "StepEndStatusId" });
            // pckg.RootRecords.Add("BusinessProcessSteps", "where bpstepid = 3138");

            //pckg.Exceptions.Add("ServiceJobItemRelationLog");
            //pckg.Exceptions.Add("ServiceJobLog");
            //pckg.Exceptions.Add("ServiceJobItemLog");
            //pckg.Exceptions.Add("ApplicationServers");
            //pckg.Exceptions.Add("ServiceJobSchedulesDependencies");

            pckg.RootRecords.Add(new RecordToExtract("BusinessProcessesTypes", "'NMT_OUT_CA_PP53'"));
            pckg.Exceptions.Add("Users");
            pckg.TablesToProcess.Add(new TableToExtract("BusinessProcessSteps", new FKDependencyExtractStrategy()));
            pckg.TablesToProcess.Add(new TableToExtract("BPTransactionTemplates", new FKDependencyExtractStrategy()));
            pckg.TablesToProcess.Add(new TableToExtract("bpProcessingConditions", new OnlyOneTableEtraction()));
            return pckg;
        }
        

        static async Task MainAsync()
        {
            var pckg = CreateDebugPackage();
       
            //"ServiceJobItemRelationLog" "ServiceJobLog" "ServiceJobItemRelationLog"
            var DependentTables = GetDependentTables();
            var MetaData = GetMetaData(new MetaDataInitializer());

            await Task.WhenAll(DependentTables, MetaData);

            var schema = new MSSQLSourceSchema(DependentTables.Result, MetaData.Result);

            var builder = new DependencyBuilder(_uowFactory, schema, _log);

            var pTables = await builder.PrepareAsync(pckg);
            var sqlBuilder = new MSSqlBuilder();

            _log.Debug(sqlBuilder.Build(pTables));

            _log.Debug(string.Empty);

            //Process(pTables);


        }

        static void Process(IEnumerable<PRecord> records)
        {
            if (records == null
                || !records.Any())
                return;

            foreach (var record in records)
            {
                ProcessOne(record, null);
            }
        }

        static void ProcessOne (PRecord item, PRecord parentRecord )
        {
            _log.DebugFormat("Item {0} ", item.ToString());
            if (parentRecord != null)
                SqlHelper.InjectSqlVariable(SqlHelper.NotIdentityFields(item), parentRecord.GetPKVarName(), parentRecord.PkField.FieldName);

            foreach (var child in item.Childern.Select(_=>_.PRecord))
                ProcessOne(child, item);
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

                var metaColumns = uof.ExecuteReaderAsync(MSSQLSourceSchema.sqlPKColumns);//uof.GetSchemaAsync("Columns");

                //var indexes = uof.ExecuteReaderAsync(MSSQLSourceSchema.sqlPKColumns);

                await Task.WhenAll(metaColumns, metaTables);

                var dt = new DataTable();
                dt.Load(metaColumns.Result);

                var iList = dt.Rows.Cast<DataRow>();

                foreach (DataRow t in metaTables.Result.Rows)
                {
                    var pTab = new PTableMetadata() { TableName = t["table_name"].ToString() };
                    //var pkField = iList.FirstOrDefault(_ => _["TableName"].ToString() == pTab.TableName)?["ColumnName"].ToString();
                    //var indx = iList.Where(_ => _["TableName"].ToString() == pTab.TableName).ToList();

                    foreach (DataRow c in iList.Where(_ => _["TableName"].ToString() == pTab.TableName))
                    {
                        var field = initializer.InitTableMetaData(c);
                      
                        pTab.Add(field);
                       
                    }
                    UniqueColumnsCollection uniqueColumns = null;
                    if (GlobalConfiguration.UniqueColums.TryGetValue(pTab.TableName,out uniqueColumns))
                        pTab.UniqueColumnsCollection.AddRange(uniqueColumns);

                    result.Add(pTab);
                }
            }

            return result;
        }
    }
}
