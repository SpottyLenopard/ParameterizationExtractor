using Quipu.ParameterizationExtractor.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Quipu.ParameterizationExtractor.Model;

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
                var rootTable = await GetPTable(root.Key, root.Value);
                rootTable.IsStartingPoint = true;
                stack.Push(rootTable);
            }
           
            while(stack.Count > 0)
            {
                _log.Debug("Start iteration");
                var record = stack.Pop();
                _log.DebugFormat("{0} is processing now. {1}", record.TableName, record.Source);
                if (!processedTables.Any(_ => _.Source == record.Source))
                {                                       
                    processedTables.Add(record);

                    foreach (var item in await GetRelatedTables(record,_=>!template.Exceptions.Contains(_)))
                    {                        
                        if (!template.Exceptions.Any(_ => _ == item.TableName))
                            stack.Push(item);
                    }

                }
                _log.Debug("End iteration");
            }

            return processedTables.Where(_ => _.IsStartingPoint).ToList();
        }

        private async Task<IEnumerable<PRecord>> GetRelatedTables(PRecord table, Func<string, bool> checkRecord)
        {
            var result = new List<PRecord>();

            if (!table.IsStartingPoint && !_schema.DependentTables.Any(_ => _.ParentTable == table.TableName))
                return result;

            var tables = _schema.DependentTables.Where(_ => _.ParentTable == table.TableName)                         
                                    .Union(_schema.DependentTables.Where(_ => _.ReferencedTable == table.TableName));
            
            foreach (var item in tables)
            {
                _log.DebugFormat("parent table: {0} referenced table {1}", item.ParentTable, item.ReferencedTable);

                Func<string, string, string, Task<PRecord>> insertTable = async (tableName, columnName, pkColumn) =>
                {
                    if (!checkRecord(tableName))
                        return await Task.FromResult<PRecord>(null);

                    var value = table.FirstOrDefault(_ => _.FieldName == columnName)?.ValueToSqlString();
                    if (value != null)
                    {
                        var str = string.Format("where {0} = {1}", pkColumn, value);
                        return await GetPTable(tableName, str);
                    }

                    return await Task.FromResult<PRecord>(null);
                };

                if (item.ParentTable == table.TableName)
                {
                    var i = await insertTable(item.ReferencedTable, item.ParentColumn, item.ReferencedColumn);
                    if (i != null)
                    {
                        table.Parents.Add(new PTableDependency() { PRecord = i, FK = item });
                        result.Add(i);
                    }
                }
                else
                {
                    var i = await insertTable(item.ParentTable, item.ReferencedColumn, item.ParentColumn);
                    if (i != null)
                    {
                        table.Childern.Add(new PTableDependency() { PRecord = i, FK = item });
                        result.Add(i);
                    }
                }
            }

            return result;
        }
      
        public async Task<PRecord> GetPTable(string tableName, string where)
        {
            var result = new List<PRecord>();

            var stringBuilder = new StringBuilder();

            stringBuilder.AppendFormat("select * from {0} ", tableName);
            if (!string.IsNullOrEmpty(where))
                stringBuilder.Append(where);

            _log.Debug(stringBuilder.ToString());

            using (var uow = _unitOfWorkFactory.GetUnitOfWork())
            {
                var reader = await uow.ExecuteReaderAsync(stringBuilder.ToString());

                while(reader.Read())
                {
                    result.Add(new PRecord(reader, _schema.Tables.First(_ => _.TableName == tableName)) { Source = stringBuilder.ToString().Trim() });
                }
            }

            return result.FirstOrDefault();
        }
    }
}
