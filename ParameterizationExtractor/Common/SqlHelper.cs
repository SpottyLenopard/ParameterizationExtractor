using Quipu.ParameterizationExtractor.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quipu.ParameterizationExtractor.Common
{
    public static class SqlHelper
    {
        public static IEnumerable<PField> NotIdentityFields(PTable table)
        {
            return table.Where(_ => !_.MetaData.IsIdentity).ToList();
        }

        public static string IfExistsSql(PTable table)
        {
            var fields = NotIdentityFields(table);

            var sql = new StringBuilder();

            sql.AppendFormat("if not exists(select top 1 1 from {0} {1})", table.TableName, table.GetUniqueSqlWhere());
            sql.AppendLine();
            sql.AppendFormat("insert into {0} (", table.TableName);
            sql.Append(string.Join(",", fields.Select(_=>_.FieldName)));           
            sql.AppendFormat(")");
            sql.AppendLine("Values(");
            sql.Append(string.Join(",", fields.Select(_ => _.ValueToSqlString())));
            sql.Append(")");

            return sql.ToString();
        }
    }
}
