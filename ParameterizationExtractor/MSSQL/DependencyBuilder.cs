using Quipu.ParameterizationExtractor.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Quipu.ParameterizationExtractor.Model;
using Quipu.ParameterizationExtractor.Common;
using ParameterizationExtractor;

namespace Quipu.ParameterizationExtractor.MSSQL
{
    public class DependencyBuilder : IDependencyBuilder
    {
        private readonly IUnitOfWorkFactory _unitOfWorkFactory;
        private readonly ISourceSchema _schema;
        private readonly ILog _log;
        public DependencyBuilder(IUnitOfWorkFactory unitOfWorkFactory, ISourceSchema schema, ILog log)
        {
            _unitOfWorkFactory = unitOfWorkFactory;
            _schema = schema;
            _log = log;
        }

        public async Task<IEnumerable<PRecord>> PrepareAsync(IPackageTemplate template)
        {
            var processedTables = new HashSet<PRecord>();
            var stack = new Stack<PRecord>();

            foreach (var root in template.RootRecords)
            {
                var rootTable = await GetPTable(root.TableName, root.PkValue);
                rootTable.IsStartingPoint = true;
                stack.Push(rootTable);
            }

            while (stack.Count > 0)
            {
                _log.Debug("Start iteration");
                var record = stack.Pop();
                _log.DebugFormat("{0} is processing now. {1}", record.TableName, record.Source);
                if (!processedTables.Any(_ => _.Equals(record)))
                {
                    processedTables.Add(record);

                    foreach (var item in await GetRelatedTables(record, template))
                    {
                        if (!template.Exceptions.Any(_ => _ == item.TableName))
                            stack.Push(item);
                    }

                }
                _log.Debug("End iteration");
            }

            return processedTables.Where(_ => _.IsStartingPoint).ToList();
        }

        private ExtractStrategy GetExtractStrategy(string tableName, IPackageTemplate template)
        {
            var fromTemplate = template.TablesToProcess.FirstOrDefault(_ => _.TableName == tableName)?.ExtractStrategy;

            return fromTemplate ?? Program.GlobalConfiguration.DefaultExtractStrategy;
        }

        private async Task<IEnumerable<PRecord>> GetRelatedTables(PRecord table, IPackageTemplate template)
        {
            var result = new List<PRecord>();

            //if (!table.IsStartingPoint && !_schema.DependentTables.Any(_ => _.ParentTable == table.TableName))
            //    return result;

            var tables = _schema.DependentTables.Where(_ => _.ParentTable == table.TableName)
                                    .Union(_schema.DependentTables.Where(_ => _.ReferencedTable == table.TableName));

            foreach (var item in tables)
            {
                _log.DebugFormat("parent table: {0} referenced table {1}", item.ParentTable, item.ReferencedTable);

                Func<string, string, string, Task<IEnumerable<PRecord>>> insertTable = async (tableName, columnName, pkColumn) =>
                {
                    //if (template.Exceptions.Any(_ => _ == tableName))
                    if (!template.TablesToProcess.Any(_ => _.TableName == tableName))
                        return await Task.FromResult<IEnumerable<PRecord>>(null);

                    var value = table.FirstOrDefault(_ => _.FieldName == columnName)?.ValueToSqlString();
                    if (value != null)
                    {
                        var str = string.Format("{0} = {1}", pkColumn, value);
                        return await GetPTables(tableName, str);
                    }

                    return await Task.FromResult<IEnumerable<PRecord>>(null);
                };
                var extractStrategy = GetExtractStrategy(table.TableName, template);

                if (item.ParentTable == table.TableName
                    && extractStrategy.ProcessParents)
                {
                    var i = (await insertTable(item.ReferencedTable, item.ParentColumn, item.ReferencedColumn))?.FirstOrDefault();
                    if (i != null)
                    {
                        table.Parents.Add(new PTableDependency() { PRecord = i, FK = item });
                        result.Add(i);
                    }
                }
                if (item.ReferencedTable == table.TableName
                    && extractStrategy.ProcessChildren)
                {
                    var i = await insertTable(item.ParentTable, item.ReferencedColumn, item.ParentColumn);
                    if (i != null)
                    {
                        foreach (var child in i)
                            table.Childern.Add(new PTableDependency() { PRecord = child, FK = item });

                        result.AddRange(i);
                    }
                }
            }

            return result;
        }

        public async Task<PRecord> GetPTable(string tableName, string objectId)
        {
            var result = new List<PRecord>();

            var tableMetaData = _schema.GetTableMetaData(tableName);

            var sql = string.Format("select * from {0} where [{1}] = {2}", tableName, tableMetaData.PK.FieldName, objectId);

            _log.DebugFormat("GetPTable : {0}", sql);

            using (var uow = _unitOfWorkFactory.GetUnitOfWork())
            {
                var reader = await uow.ExecuteReaderAsync(sql);

                while (reader.Read())
                {
                    result.Add(new PRecord(reader, tableMetaData) { Source = sql.Trim() });
                }
            }

            return result.FirstOrDefault();
        }

        public async Task<IEnumerable<PRecord>> GetPTables(string tableName, string where)
        {
            var result = new List<PRecord>();
            var sql = string.Format("select * from {0} where {1}", tableName, where);
            _log.DebugFormat("GetPTables : {0}", sql);

            using (var uow = _unitOfWorkFactory.GetUnitOfWork())
            {
                var reader = await uow.ExecuteReaderAsync(sql);

                while (reader.Read())
                {
                    result.Add(new PRecord(reader, _schema.GetTableMetaData(tableName)) { Source = sql.Trim() });
                }
            }

            return result;
        }                
    }
}
