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

        public static IEnumerable<PField> InjectSqlVariable(IEnumerable<PField> fields, string sqlVar, string fieldName)
        {
            var copy = new List<PField>(fields);
            var f = copy.FirstOrDefault(_ => _.FieldName == fieldName);

            if (f != null)
                f.Expression = sqlVar;

            return copy;
        }

        public static IEnumerable<PField> PrepareFieldsForChild(PTable child, string sqlVar, PDependentTable fk)
        {
            return InjectSqlVariable(NotIdentityFields(child), sqlVar, fk.ReferencedColumn);
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
        public static bool IsNumericType(this object o)
        {
            return o.GetType().IsNumericType();
        }
        public static bool IsNumericType(this Type o)
        {
            switch (Type.GetTypeCode(o))
            {
                case TypeCode.Byte:
                case TypeCode.SByte:
                case TypeCode.UInt16:
                case TypeCode.UInt32:
                case TypeCode.UInt64:
                case TypeCode.Int16:
                case TypeCode.Int32:
                case TypeCode.Int64:
                case TypeCode.Decimal:
                case TypeCode.Double:
                case TypeCode.Single:
                    return true;
                default:
                    return false;
            }
        }
    }


}
